﻿using BepInEx.Logging;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MoreSlugcats;
using UnityEngine;
using Expedition;
using RWCustom;
using System.Globalization;
namespace ThePatriarch;
public partial class Hooks
{
    private static BindingFlags propFlags = BindingFlags.Instance | BindingFlags.Public;
    private static BindingFlags methodFlags = BindingFlags.Static | BindingFlags.Public;
    public delegate bool orig_RegionGate_MeetRequirement(RegionGate self);
    public static void ApplyPearlHook()
    {
        On.Region.GetProperRegionAcronym += Region_GetProperRegionAcronym;

        //业力门相关（还有一个地图显示业力门符号的方法在HUD里）
        On.RegionGate.KarmaBlinkRed += RegionGate_KarmaBlinkRed;
        On.GateKarmaGlyph.Update += GateKarmaGlyph_Update;
        Hook hook = new Hook(typeof(RegionGate).GetProperty("MeetRequirement", propFlags).GetGetMethod(), typeof(Hooks).GetMethod("RegionGate_get_MeetRequirement", methodFlags));
    }
    public static bool RegionGate_KarmaBlinkRed(On.RegionGate.orig_KarmaBlinkRed orig, RegionGate self)
    {
        bool result = orig(self);
        if (ModManager.MSC && self.karmaRequirements[self.letThroughDir ? 0 : 1] == Enums.PatriarchLock)
        {
            result = false;
        }
        return result;
    }

    private static void GateKarmaGlyph_Update(On.GateKarmaGlyph.orig_Update orig, GateKarmaGlyph self, bool eu)
    {
        orig.Invoke(self, eu);
        //NSH封锁的业力门
        if (ModManager.MSC && self.requirement == Enums.PatriarchLock)
        {
            self.redSine += 1f;
            self.col = new Color(1f, Mathf.Sin(self.redSine / 25f) * 0.5f + 0.5f, Mathf.Sin(self.redSine / 25f) * 0.5f + 0.5f);
        }
        //珍珠打开业力门
        if (ModManager.MSC && ShouldPlayAnimation(self) != 0)
        {
            if (PearlWritedSave.pearlWrited && (openCount > 0 || openGate))
            {
                /*
                if (animationTicker % 3 == 0 && !self.animationFinished)
                {
                    self.animationIndex++;
                }
                if (animationTicker % 15 == 0)
                {
                    self.glyphIndex++;
                    if (self.glyphIndex < 10)
                    {
                        self.room.PlaySound(MoreSlugcatsEnums.MSCSoundID.Data_Bit, self.pos, 1f, 0.5f + UnityEngine.Random.value * 2f);
                    }
                }
                if (self.animationIndex > 9)
                {
                    self.animationIndex = 0;
                }
                if (self.glyphIndex >= 10)
                {
                    self.animationFinished = true;
                }
                else
                {
                    self.animationFinished = false;
                    Vector2 pos = self.pos;
                    pos.x += (float)(self.glyphIndex % 3 * 9) - 8f;
                    pos.y += (float)(self.glyphIndex / 3 * 9) - 5f;
                }
                if (self.animationFinished && self.mismatchLabel != null && ShouldPlayAnimation(self) < 0)
                {
                    self.mismatchLabel.NewPhrase(51);
                }*/
                if (openCount < 40)
                {
                    self.col = Color.Lerp(self.col, new Color(1f, 0f, 0f), ((float)openCount) / 40f);
                }
                else if (openCount >= 40 && openCount < 225)
                {
                    if (openCount % 20 <= 5)
                    {
                        self.col = Color.Lerp(self.col, new Color(0f, 1f, 0f), ((float)openCount % 20) / 5f);
                    }
                    else
                    {
                        self.col = Color.Lerp(self.col, new Color(1f, 0f, 0f), ((float)openCount % 20 - 5f) / 15f);
                    }
                }
                else if (openCount <= 300)
                {
                    self.col = Color.Lerp(self.col, new Color(0f, 1f, 0f), Mathf.Min(((float)openCount - 225f) / 40f, 1f));
                }
                if (openGate && openGateName == self.room.abstractRoom.name)
                {
                    self.col = new Color(0f, 1f, 0f);
                }
            }
        }
    }

    public static bool RegionGate_get_MeetRequirement(Hooks.orig_RegionGate_MeetRequirement orig, RegionGate self)
    {
        AbstractCreature firstAlivePlayer = self.room.game.FirstAlivePlayer;
        if (self.room.game.Players.Count == 0 || firstAlivePlayer == null || (firstAlivePlayer.realizedCreature == null && ModManager.CoopAvailable))
        {
            return false;
        }
        Player player;
        if (ModManager.CoopAvailable && self.room.game.AlivePlayers.Count > 0)
        {
            player = (self.room.game.FirstAlivePlayer.realizedCreature as Player);
        }
        else
        {
            player = (self.room.game.Players[0].realizedCreature as Player);
        }
        if (player == null)
        {
            return false;
        }
        bool result = orig(self);
        //NSH封锁了业力门
        if (self.room.world.region.name == "NSH" &&
            self.room.abstractRoom.name.Contains("AVA") ||
            (self.room.abstractRoom.name.Contains("DGL") && self.karmaRequirements[(!self.letThroughDir) ? 1 : 0] == Enums.PatriarchLock))
        {
            return !Plugin.gateLock;
        }
        //珍珠解锁业力门
        if (IsPatriarch(self.room.game) &&
            openGate && openGateName == self.room.abstractRoom.name && self.EnergyEnoughToOpen)
        {
            return true;
        }
        return result;
    }

    public static string Region_GetProperRegionAcronym(On.Region.orig_GetProperRegionAcronym orig, SlugcatStats.Name character, string baseAcronym)
    {
        string result = orig(character, baseAcronym);
        var msg = character + " " + Plugin.CAT_NAME;
        Debug.Log(msg);
        if (character.ToString() == Plugin.CAT_NAME && PearlWritedSave.pearlWrited && openGate)
        {
            string text = baseAcronym;
            if (text == "OE")
            {
                text = "NSH";
            }
            string[] array = AssetManager.ListDirectory("World", true, false);
            for (int i = 0; i < array.Length; i++)
            {
                string path = AssetManager.ResolveFilePath(string.Concat(new string[]
                {
                        "World",
                        Path.DirectorySeparatorChar.ToString(),
                        Path.GetFileName(array[i]),
                        Path.DirectorySeparatorChar.ToString(),
                        "equivalences.txt"
                }));
                if (File.Exists(path))
                {
                    string[] array2 = File.ReadAllText(path).Trim().Split(new char[]
                    {
                            ','
                    });
                    for (int j = 0; j < array2.Length; j++)
                    {
                        string text2 = null;
                        string a = array2[j];
                        if (array2[j].Contains("-"))
                        {
                            a = array2[j].Split(new char[]
                            {
                                    '-'
                            })[0];
                            text2 = array2[j].Split(new char[]
                            {
                                    '-'
                            })[1];
                        }
                        if (a == baseAcronym && (text2 == null || character.value.ToLower() == text2.ToLower()))
                        {
                            text = Path.GetFileName(array[i]).ToUpper();
                        }
                    }
                }
            }
            result = text;
        }
        return result;
    }

    public static int ShouldPlayAnimation(GateKarmaGlyph self)
    {/*
            if (!ModManager.MSC || self.requirement != MoreSlugcatsEnums.GateRequirement.OELock)
            {
                return 0;
            }*/
        if (self.gate.mode != RegionGate.Mode.MiddleClosed || !self.gate.EnergyEnoughToOpen || self.gate.unlocked)// || self.gate.letThroughDir == self.side
        {
            return 0;
        }
        int num = self.gate.PlayersInZone();
        /*if (num <= 0 || num >= 3)
        {
            return 0;
        }*/
        self.gate.letThroughDir = (num == 1);
        if (self.gate.dontOpen || self.gate.MeetRequirement)
        {
            return 1;
        }
        return -1;
    }
}