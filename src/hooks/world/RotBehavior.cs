using MonoMod.RuntimeDetour;
using SlugBase.Features;
using System.Collections.Generic;
using MonoMod.RuntimeDetour;
using MoreSlugcats;
using SlugBase.Features;
using UnityEngine;
using static SlugBase.Features.FeatureTypes;
using static System.Reflection.BindingFlags;
using System;
using System.Runtime.CompilerServices;
using Random = UnityEngine.Random;
using IL;
using System.Linq;
using UnityEngine.Diagnostics;
using On;
using System.Net.Configuration;
using RewiredConsts;
using UnityEngine.Rendering;
using RWCustom;
using System.Xml.XPath;
using ThePatriarch.effects;
using static DataPearl.AbstractDataPearl;

namespace ThePatriarch;
public partial class Hooks
{
    private static ConditionalWeakTable<Oracle, StrongBox<int>> OracleCWT = new ConditionalWeakTable<Oracle, StrongBox<int>>();
    //private static ConditionalWeakTable<MoreSlugcats.SSOracleRotBehavior, OracleRMModule> OracleRMData { get; } = new();
    public static readonly GameFeature<bool> CustomConversations = GameBool("CustomConversations");
    public static void ApplyPebbles()
    {
        new Hook(typeof(SSOracleRotBehavior).GetMethod("get_EyesClosed", Public | NonPublic | Instance), (Func<SSOracleRotBehavior, bool> orig, SSOracleRotBehavior self) => (self.oracle.stun > 0 && self.oracle.room.game.session is StoryGameSession session && session.saveStateNumber.value == Plugin.CAT_NAME) || orig(self));

        On.Oracle.Update += OracleUpdate;
        On.Player.Update += PlayerUpdate;
        On.MoreSlugcats.SSOracleRotBehavior.RMConversation.AddEvents += PebblesOverride;
    }

    public static void OracleUpdate(On.Oracle.orig_Update orig, Oracle self, bool eu)
    {
        orig(self, eu);
        //var game = (RWCustom.Custom.rainWorld.processManager.currentMainLoop as RainWorldGame);
        if (self.room.game.IsPatriarch())
        {
            if (OracleCWT.TryGetValue(self, out var Counter) && self.oracleBehavior is SSOracleRotBehavior rotBehavior && rotBehavior.conversation?.events != null && rotBehavior.conversation.id == MoreSlugcatsEnums.ConversationID.Pebbles_RM_FirstMeeting)
            {
                Counter.Value++;
                const int StartPain = 1196;
                const int GiveMark = 1;
                const int FixPearl = 3670;
                const int StartSecondPain = FixPearl + 700;

                const int test = 100;
                if (Counter.Value == StartPain)
                {
                    self.room.AddObject(new PanicDisplay(self));
                }
                if (Counter.Value == GiveMark)
                {
                    self.room.AddObject(new PebblesGiveMark(self));
                }
                if (Counter.Value == FixPearl)
                {
                    self.room.AddObject(new PebblesFixPearl(self));
                }
                // Combine fixpearl and second panic display
                if (Counter.Value == StartSecondPain)
                {
                    self.room.AddObject(new PebblesPanicDisplay(self));
                }
            }
        }
    }



    public static void PebblesOverride(On.MoreSlugcats.SSOracleRotBehavior.RMConversation.orig_AddEvents orig, SSOracleRotBehavior.RMConversation self)
    {
        orig(self);
        var game = (RWCustom.Custom.rainWorld.processManager.currentMainLoop as RainWorldGame);
        if (CustomConversations.TryGet(self.owner.oracle.room.game, out bool custom) && custom && self.owner.oracle.room.game.session is StoryGameSession session && session.saveState.saveStateNumber.value == Plugin.CAT_NAME)
        {
            if (PearlWritedSave.pearlWrited)
            {
                self.events = new List<Conversation.DialogueEvent>() {
                        new Conversation.TextEvent(self, 1000, "...", 103),
                        new Conversation.TextEvent(self, 5, Translate("Now you can understand meeeeeee-"), 20),
                        new Conversation.TextEvent(self, 0, "......", 60),
                        new Conversation.TextEvent(self, 5, "...", 40),
                        new Conversation.TextEvent(self, 0, Translate("Giving you mark of communication wasn't good idea."), 90),
                        new Conversation.TextEvent(self, 0, Translate("This caused some ... consequences."), 80),
                        new Conversation.TextEvent(self, 0, Translate("This mark is the only thing I can offer you. I cannot help you. I can't even help myself."), 120),
                        new Conversation.TextEvent(self, 0, Translate("I've long since lost control of most of my facilities, and my construction is rotten through.."), 130),
                        new Conversation.TextEvent(self, 0, Translate("The most of my generators has also reached the end of its operating life."), 110),
                        new Conversation.TextEvent(self, 0, Translate("How you've made it this far alive, I do not know..."), 90),
                        new Conversation.TextEvent(self, 0, Translate("But if you value your life, leave and never return."), 80),
                        new Conversation.TextEvent(self, 200, Translate("..."), 20),
                        new Conversation.TextEvent(self, 0, Translate("What?"), 80),
                        new Conversation.TextEvent(self, 0, Translate("..."), 20),
                        new Conversation.TextEvent(self, 0, Translate("Looks like you smart enough for trying to use mark to ... transfer some data?"), 110),
                        new Conversation.TextEvent(self, 0, Translate("Let me just..."), 90),
                        new Conversation.TextEvent(self, 730, Translate("..."), 30),
                        new Conversation.TextEvent(self, 0, Translate("If I correctly interpret the mental images you conveyed, your home was flooded due to my activity."), 130),
                        new Conversation.TextEvent(self, 0, Translate("I think I can help you."), 20),
                        new Conversation.TextEvent(self, 0, Translate("I once met someone of your kind. This creature carried with it an important cargo and a data pearl.<LINE>I managed to copy its contents."), 150),
                        new Conversation.TextEvent(self, 0, Translate("That pearl contained a sequence that targeted the Karma Gates. Theoretically, it could open any gate by interfering its current."), 150),
                        new Conversation.TextEvent(self, 0, Translate("I'll try to write this sequence on one of my pearls.<LINE>Wait a bit..."), 110),
                        new Conversation.TextEvent(self, 600, Translate("..."), 20),
                        new Conversation.TextEvent(self, 0, Translate("Sequence writing was succeeeeeeeee-"), 70),
                        new Conversation.TextEvent(self, 0, Translate("..."), 20),
                        new Conversation.TextEvent(self, 0, Translate("Take this pearl and find a new home for your tribe. Then for your own sake, never return here."), 130)
                    };
            }
            else
            {
                self.events = new List<Conversation.DialogueEvent>() {
                        new Conversation.TextEvent(self, 0, Translate("You again? I have nothing for you. Leave."), 103)
                    };
            }
        }
    }

    public class PebblesGiveMark : UpdatableAndDeletable
    {
        Oracle self;
        private int timer;
        public PebblesGiveMark(Oracle self)
        {
            this.self = self;
        }

        public override void Update(bool eu)
        {
            var game = (RWCustom.Custom.rainWorld.processManager.currentMainLoop as RainWorldGame);
            base.Update(eu);
            timer++;
            if (timer == 50)
            {
                self.room.PlaySound(SoundID.SS_AI_Talk_4, self.firstChunk, false, 1f, 1f);
            }
            if (timer == 150)
            {
                self.room.PlaySound(SoundID.SS_AI_Talk_3, self.firstChunk, false, 1f, 1f);
            }
            if (timer == 170)
            {
                self.room.PlaySound(SoundID.SS_AI_Talk_3, self.firstChunk, false, 1f, 1f);
            }
            if (timer == 250)
            {
                self.room.PlaySound(SoundID.SS_AI_Talk_2, self.firstChunk, false, 1f, 1f);
            }
            if (timer == 600)
            {
                self.room.PlaySound(SoundID.SS_AI_Talk_1, self.firstChunk, false, 1f, 1f);
            }
            if (timer == 800)
            {
                for (int i = 1; i < 20; i++)
                {
                    bool lockToHorizontal = Random.Range(0f, 1f) > 0.5f;
                    Spark sparkPlayer = new Spark(playerPos, RWCustom.Custom.rotateVectorDeg(Vector2.down * (i * 2.5f), Random.Range(0f, 360f)), Color.white, null, 10, 30);
                    //Spark sparkOracle = new Spark(self.RandomChunk.pos, RWCustom.Custom.rotateVectorDeg(Vector2.down * (i * 2.5f), Random.Range(0f, 360f)), Color.white, null, 10, 30);
                    self.room.AddObject(sparkPlayer);
                    //self.room.AddObject(sparkOracle);
                }
                game.GetStorySession.saveState.deathPersistentSaveData.theMark = true;
                self.room.game.FirstAlivePlayer.realizedCreature.stun = 100;
                self.room.PlaySound(SoundID.SS_AI_Give_The_Mark_Boom, self.firstChunk, false, 1f, 1f);
            }
        }
    }
    public class PebblesFixPearl : UpdatableAndDeletable
    {
        Oracle self;
        private int timer;
        private DataPearl pearl1;
        private DataPearl pearl;
        private Vector2 wantPos = new Vector2(1530.1f, 800.5f);
        private DataPearl.AbstractDataPearl writedPearl;

        //var oracleModule = OracleRMData.GetValue(self, _ => new OracleRMModule());
        public PebblesFixPearl(Oracle self)
        {
            this.self = self;
            //self.room.PlaySound(SoundID.SS_AI_Talk_1, self.firstChunk, false, 1f, 1f);
            timer = 0;
            pearl = null;
        }

        public override void Update(bool eu)
        {
            base.Update(eu);
            timer++;
            if (timer == 1)
            {
                Debug.Log("Seaching for pearl to fix");
                List<PhysicalObject>[] physicalObjects = self.room.physicalObjects;
                for (int i = 0; i < physicalObjects.Length; i++)
                {
                    for (int j = 0; j < physicalObjects[i].Count; j++)
                    {
                        PhysicalObject physicalObject = physicalObjects[i][j];
                        if (physicalObject is DataPearl && (!(physicalObject is HalcyonPearl)))
                        {
                            pearl1 = physicalObject as DataPearl;
                            if (pearl1.color.r == 0.7f)
                            {
                                pearl = physicalObject as DataPearl;
                            }
                        }
                    }
                }
                pearl.firstChunk.HardSetPosition(wantPos);
                //myPearl = CustomRegions.Mod.CustomWorldMod.customPearls.GetEnumerator()
                //pearl.AbstractDataPearl.DataPearlType = CustomRegions.Mod.CustomWorldMod.customPearls.;

            }
            if (timer > 1 && timer < 600 && pearl != null)
            {
                if (timer % 20 == 0)
                {
                    self.room.PlaySound(MoreSlugcatsEnums.MSCSoundID.Data_Bit, pearl.firstChunk.pos, 1f, 1f + Random.value * 2f);
                    self.room.AddObject(new Explosion.ExplosionLight(pearl.firstChunk.pos, 150f, 1f, 15, Color.blue));
                    for (int i = 1; i < 4; i++)
                    {
                        self.room.AddObject(new Spark(pearl.firstChunk.pos, RWCustom.Custom.rotateVectorDeg(Vector2.down * (i * 2.5f), Random.Range(0f, 360f)), Color.blue, null, 20, 40));
                    }
                }
                pearl.firstChunk.vel *= Custom.LerpMap(pearl.firstChunk.vel.magnitude, 1f, 6f, 0.999f, 0.9f);
                pearl.firstChunk.vel += Vector2.ClampMagnitude(wantPos - pearl.firstChunk.pos, 100f) / 100f * 0.4f;
                //抵消重力
                pearl.firstChunk.vel += 0.6f * Vector2.up;
                //随机速度
                pearl.firstChunk.vel += (Random.value - 0.5f) * 0.05f * Vector2.up;
                pearl.color.r = pearl.color.r - 0.0012f;
                pearl.color.g = pearl.color.g - 0.0012f;
            }
            if (timer == 600 && pearl != null)
            {
                self.room.PlaySound(SoundID.Moon_Wake_Up_Green_Swarmer_Flash, pearl.firstChunk.pos, 0.5f, 1f);
                self.room.AddObject(new ElectricFullScreen.SparkFlash(pearl.firstChunk.pos, 50f));
            }
            if (timer == 620)
            {
                PearlWritedSave.pearlWrited = true;
                pearl.RemoveFromRoom();
                WorldCoordinate writedPearlPos = pearl.AbstractPearl.pos;
                writedPearl = new DataPearl.AbstractDataPearl(self.room.world, AbstractPhysicalObject.AbstractObjectType.DataPearl, null, writedPearlPos, self.room.game.GetNewID(), -1, -1, null, Enums.FixedPebblesPearl);
                writedPearl.RealizeInRoom();
                self.room.AddObject(new WritedDataPearlEffect(pearl, self.room));
            }

        }
    }
    public class PebblesPanicDisplay : UpdatableAndDeletable
    {
        Oracle oracle;
        private int[] timings;
        private int timer;
        public PebblesPanicDisplay(Oracle oracle)
        {
            this.oracle = oracle;
            timings = new int[] {
                    140,
                    190,
                    210,
                    240,
                    320,
                    30
                };
            oracle.stun = 300;
            oracle.dazed = 300;
            (oracle.oracleBehavior as SSOracleRotBehavior)?.AirVoice(SoundID.SS_AI_Talk_4);
            oracle.room.PlaySound(SoundID.SL_AI_Pain_1, oracle.firstChunk, false, 0.7f, 0.3f);
            oracle.bodyChunks[1].vel.x -= 5f;
            Debug.Log(oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.DarkenLights)?.amount);
            Debug.Log(oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Darkness)?.amount);
            Debug.Log(oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Contrast)?.amount);
        }
        public override void Update(bool eu)
        {
            base.Update(eu);
            timer++;
            if (timer == 1)
            {
                if (oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.DarkenLights) == null)
                {
                    oracle.room.roomSettings.effects.Add(new RoomSettings.RoomEffect(RoomSettings.RoomEffect.Type.DarkenLights, 0f, false));
                }
                if (oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Darkness) == null)
                {
                    oracle.room.roomSettings.effects.Add(new RoomSettings.RoomEffect(RoomSettings.RoomEffect.Type.Darkness, 0f, false));
                }
                if (oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Contrast) == null)
                {
                    oracle.room.roomSettings.effects.Add(new RoomSettings.RoomEffect(RoomSettings.RoomEffect.Type.Contrast, 0f, false));
                }
                oracle.room.PlaySound(SoundID.Broken_Anti_Gravity_Switch_Off, 0f, 1f, 1f);
            }
            if (timer < timings[0])
            {
                float t = (float)timer / timings[0];
                oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.DarkenLights).amount = Mathf.Lerp(0f, 0.6f, t);
                oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Darkness).amount = Mathf.Lerp(0f, 0.4f, t);
                oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Contrast).amount = Mathf.Lerp(0f, 0.3f, t);
            }
            if (timer == timings[0])
            {
                oracle.arm.isActive = false;
                oracle.setGravity(0.9f);
                oracle.stun = 9999;
                //oracle.room.PlaySound(MoreSlugcatsEnums.MSCSoundID.Moon_Panic_Attack, 0f, 0.85f, 1f);
                //oracle.room.PlaySound(SoundID.
                for (int i = 0; i < oracle.room.game.cameras.Length; i++)
                {
                    if (oracle.room.game.cameras[i].room == oracle.room && !oracle.room.game.cameras[i].AboutToSwitchRoom)
                    {
                        oracle.room.game.cameras[i].ScreenMovement(null, Vector2.zero, 15f);
                    }
                }
            }
            if (timer == (timings[1] + timings[2]) / 2)
            {
                oracle.arm.isActive = false;
                // oracle.room.PlaySound((Random.value < 0.5f) ? SoundID.SL_AI_Pain_1 : SoundID.SL_AI_Pain_2, 0f, 0.5f, 1f);
                // chatLabel = new OracleChatLabel(oracle.oracleBehavior);
                // chatLabel.pos = new Vector2(485f, 360f);
                // chatLabel.NewPhrase(99);
                oracle.setGravity(0.9f);
                oracle.stun = 9999;
                // oracle.room.AddObject(chatLabel);
            }
            if (timer > timings[1] && timer < timings[2] && timer % 5 == 0)
            {
                oracle.room.ScreenMovement(null, new Vector2(0f, 0f), 2.5f);
                // for (int j = 0; j < 6; j++)
                // {
                //     if (Random.value < 0.5f)
                //     {
                //         oracle.room.AddObject(new OraclePanicDisplay.PanicIcon(new Vector2((float)Random.Range(230, 740), (float)Random.Range(100, 620))));
                //     }
                // }
            }
            if (timer >= timings[2] && timer <= timings[3])
            {
                oracle.room.ScreenMovement(null, new Vector2(0f, 0f), 1f);
            }
            if (timer == timings[3])
            {
                // chatLabel.Destroy();
                oracle.room.PlaySound(SoundID.Broken_Anti_Gravity_Switch_On, 0f, 1f, 1f);
            }
            if (timer > timings[3])
            {
                float t2 = (float)(timer - timings[3]) / (int)(timings[0] / 2.1f);
                oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.DarkenLights).amount = Mathf.Lerp(0.6f, 0f, t2);
                oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Darkness).amount = Mathf.Lerp(0.4f, 0f, t2);
                oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Contrast).amount = Mathf.Lerp(0.3f, 0f, t2);
            }
            if (timer == timings[3] + (int)(timings[0] / 2.1f))
            {
                oracle.setGravity(0f);
                oracle.arm.isActive = true;
                oracle.stun = 50;
            }
            if (timer > timings[3] + (int)(timings[0] / 2.1f) && timer < timings[4])
            {
                oracle.bodyChunks[0].vel += new Vector2(Random.Range(-1.7f, 1.7f), Random.Range(-1.2f, 1.2f));
                oracle.bodyChunks[0].vel *= 0.3f;
            }
            if (timer >= timings[4])
            {
                oracle.room.roomSettings.effects.RemoveAll(eff => eff.type == RoomSettings.RoomEffect.Type.DarkenLights || eff.type == RoomSettings.RoomEffect.Type.Darkness || eff.type == RoomSettings.RoomEffect.Type.Contrast);
                Destroy();
            }
            if (timer == timings[5])
            {
                (oracle.oracleBehavior as SSOracleRotBehavior)?.AirVoice(SoundID.SS_AI_Talk_1);
                oracle.room.PlaySound(SoundID.SL_AI_Pain_2, oracle.firstChunk, false, 0.87f, 0.45f);
            }
        }
    }
    public class PanicDisplay : UpdatableAndDeletable
    {
        Oracle oracle;
        private int timer;
        private int[] timings;

        public PanicDisplay(Oracle oracle)
        {
            this.oracle = oracle;
            timings = new int[5] {35, 155, 200, 250, 300 };
            oracle.stun = 300;
            oracle.dazed = 300;
            //oracle.room.PlaySound(SoundID.SL_AI_Pain_1, oracle.firstChunk, false, 0.7f, 0.3f);
            oracle.bodyChunks[1].vel.x -= 5f;
        }

        public override void Update(bool eu)
        {
            base.Update(eu);
            ++timer;
            if (timer < timings[0] && timer % 7 == 0)
            {
                for (int i = 1; i < 3; i++)
                {
                    bool lockToHorizontal = Random.Range(0f, 1f) > 0.5f;
                    Spark spark = new Spark(new Vector2(lockToHorizontal ? (Random.Range(0f, 1f) > 0.5f ? 1225 : 1795) : Random.Range(1225, 1795), !lockToHorizontal ? (Random.Range(0f, 1f) > 0.5f ? 805 : 1375) : Random.Range(800, 1375)), RWCustom.Custom.rotateVectorDeg(Vector2.down * (i * 2.5f), Random.Range(0f, 360f)), Color.yellow, null, 10, 30);
                    oracle.room.AddObject(spark);

                    if (i == 1 && timer % 2 == 0)
                    {
                        lockToHorizontal = Random.Range(0f, 1f) > 0.5f;
                        Vector2 startPos = new Vector2(lockToHorizontal ? (Random.Range(0f, 1f) > 0.5f ? 1225 : 1795) : Random.Range(1225, 1795), !lockToHorizontal ? (Random.Range(0f, 1f) > 0.5f ? 805 : 1375) : Random.Range(800, 1375));
                        Vector2 endPos = new Vector2(!lockToHorizontal ? (Random.Range(0f, 1f) > 0.5f ? 1225 : 1795) : Random.Range(1225, 1795), lockToHorizontal ? (Random.Range(0f, 1f) > 0.5f ? 805 : 1375) : Random.Range(800, 1375));
                        LightningBolt lightning = new LightningBolt(startPos, endPos, 0, 0.2f, 1, 0, 0f, false);
                        lightning.intensity = 1f;
                        lightning.color = new Color(Random.Range(0.85f, 1f), Random.Range(0f, 0.196f), Random.Range(0f, 0.456f));
                        oracle.room.AddObject(lightning);
                        oracle.room.PlaySound(SoundID.Zapper_Zap, lightning.from, 0.6f, 1f);
                        oracle.room.PlaySound(SoundID.Zapper_Disrupted_LOOP, lightning.target, 0.6f, 1f);
                    }
                }
            }       
            if (this.timer == timings[0])
            {
                oracle.bodyChunks[0].vel += new Vector2(Random.Range(-3.65f, 3.65f), Random.Range(-1f, 1f));
                if (this.oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.DarkenLights) == null)
                    this.oracle.room.roomSettings.effects.Add(new RoomSettings.RoomEffect(RoomSettings.RoomEffect.Type.DarkenLights, 0.0f, false));
                if (this.oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Darkness) == null)
                    this.oracle.room.roomSettings.effects.Add(new RoomSettings.RoomEffect(RoomSettings.RoomEffect.Type.Darkness, 0.0f, false));
                if (this.oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Contrast) == null)
                    this.oracle.room.roomSettings.effects.Add(new RoomSettings.RoomEffect(RoomSettings.RoomEffect.Type.Contrast, 0.0f, false));
                this.oracle.room.PlaySound(SoundID.Broken_Anti_Gravity_Switch_Off, 0.0f, 1f, 1f);
            }
            if (timings[0] < timer && timer < timings[1])
            {
                float t = (float)(this.timer - 35) / (float)this.timings[1];
                this.oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.DarkenLights).amount = Mathf.Lerp(0.0f, 0.6f, t);
                this.oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Darkness).amount = Mathf.Lerp(0.0f, 0.4f, t);
                this.oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Contrast).amount = Mathf.Lerp(0.0f, 0.3f, t);
            }
            if (this.timer == this.timings[1])
            {
                this.oracle.arm.isActive = false;
                this.oracle.setGravity(0.9f);
                this.oracle.stun = 9999;
                oracle.room.PlaySound(MoreSlugcatsEnums.MSCSoundID.Moon_Panic_Attack, 0f, 0.85f, 1f);
                for (int index = 0; index < this.oracle.room.game.cameras.Length; ++index)
                {
                    if (this.oracle.room.game.cameras[index].room == this.oracle.room && !this.oracle.room.game.cameras[index].AboutToSwitchRoom)
                        this.oracle.room.game.cameras[index].ScreenMovement(null, Vector2.zero, 15f);
                }
            }
            if (this.timer == (this.timings[2] + this.timings[3]) / 2)
            {
                this.oracle.arm.isActive = false;
                this.oracle.setGravity(0.9f);
                this.oracle.stun = 9999;
            }

            if (this.timer > this.timings[2] && this.timer < this.timings[3] && this.timer % 16 == 0)
            {
                this.oracle.room.ScreenMovement(new Vector2?(), new Vector2(0.0f, 0.0f), 2.5f);
                for (int index = 0; index < 6; ++index)
                {
                    if ((double)Random.value < 0.5)
                        this.oracle.room.AddObject((UpdatableAndDeletable)new OraclePanicDisplay.PanicIcon(new Vector2((float)Random.Range(230, 740), (float)Random.Range(100, 620))));
                }
            }
            if (this.timer >= this.timings[3] && this.timer <= this.timings[4])
                this.oracle.room.ScreenMovement(new Vector2?(), new Vector2(0.0f, 0.0f), 1f);

            if (this.timer == this.timings[4])
            {
                this.oracle.room.PlaySound(SoundID.Broken_Anti_Gravity_Switch_On, 0.0f, 1f, 1f);
            }
            if (this.timer > this.timings[4])
            {
                float t = (float)(this.timer - this.timings[4]) / (float)this.timings[1];
                this.oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.DarkenLights).amount = Mathf.Lerp(1f, 0.0f, t);
                this.oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Darkness).amount = Mathf.Lerp(0.4f, 0.0f, t);
                this.oracle.room.roomSettings.GetEffect(RoomSettings.RoomEffect.Type.Contrast).amount = Mathf.Lerp(0.3f, 0.0f, t);
            }
            if (this.timer != this.timings[4] + this.timings[1])
                return;
            this.oracle.setGravity(0.0f);
            this.oracle.arm.isActive = true;
            this.oracle.stun = 0;
            this.Destroy();
        }
    }
    public class PanicIcon : CosmeticSprite
    {
        private int timer;
        public float circleScale;

        public PanicIcon(Vector2 position)
        {
            this.pos = position;
        }

        public override void Update(bool eu)
        {
            this.circleScale = Mathf.Lerp(this.circleScale, 1f, 0.1f);
            if ((double)this.circleScale > 0.98000001907348633)
                ++this.timer;
            if (this.timer == 160)
                this.Destroy();
            base.Update(eu);
        }

        public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[3];
            for (int index = 0; index < 2; ++index)
            {
                sLeaser.sprites[index] = new FSprite("Futile_White");
                sLeaser.sprites[index].shader = rCam.room.game.rainWorld.Shaders["VectorCircle"];
                sLeaser.sprites[index].scale = 0.0f;
            }
            sLeaser.sprites[1].color = new Color(0.003921569f, 0.0f, 0.0f);
            sLeaser.sprites[0].color = new Color(0.0f, 0.0f, 0.0f);
            sLeaser.sprites[2] = new FSprite("miscDangerSymbol");
            sLeaser.sprites[2].isVisible = false;
            sLeaser.sprites[2].color = new Color(0.0f, 0.0f, 0.0f);
            this.AddToContainer(sLeaser, rCam, rCam.ReturnFContainer("BackgroundShortcuts"));
        }

        public override void DrawSprites(
          RoomCamera.SpriteLeaser sLeaser,
          RoomCamera rCam,
          float timeStacker,
          Vector2 camPos)
        {
            bool flag = true;
            if (this.timer > 130 && this.timer % 8 < 4)
                flag = false;
            for (int index = 0; index < 2; ++index)
            {
                sLeaser.sprites[index].x = this.pos.x - camPos.x;
                sLeaser.sprites[index].y = this.pos.y - camPos.y;
                sLeaser.sprites[index].scale = (float)((double)this.circleScale * 4.0 * (index == 0 ? 1.0 : 0.89999997615814209));
                sLeaser.sprites[index].isVisible = flag;
            }
            sLeaser.sprites[2].x = this.pos.x - camPos.x;
            sLeaser.sprites[2].y = this.pos.y - camPos.y;
            if ((double)this.circleScale > 0.98000001907348633)
                sLeaser.sprites[2].isVisible = flag;
            base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
        }
    }
}