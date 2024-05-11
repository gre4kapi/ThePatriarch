using System;
using System.Collections.Generic;
using BepInEx.Logging;
using RWCustom;
using UnityEngine;

namespace TheLeader;
public partial class Hooks
{
    private static List<Room> modifiedRooms = new List<Room>();
    static readonly Dictionary<string, float> swampRooms = new Dictionary<string, float>()
    {
        {"OE_FINAL03", 150f },
        {"OE_FINAL02", 800f },
        {"OE_JUNGLE03", 500f },
        {"OE_CAVE12", 800f },
        {"OE_TOWER08", 1600f },
        //{"OE_RUIN07", 800f },
        {"OE_JUNGLE01", 400f },
        {"OE_CAVE07", 800f },
        {"OE_WORMPIT", 500f },
        {"OE_RUIN06", 850f },
        {"OE_CAVE01", 800f },
        {"OE_CAVE05", 800f },
        {"OE_CAVE08", 800f },
        {"OE_BROKENDRAIN", 800f },
        {"OE_TOWER07", 800f },
        {"OE_CAVE15", 800f },
        {"OE_CAVE20", 800f },
        
        // Full Water
        {"OE_CAVE14", 800f },
        {"OE_CAVE13", 800f },
        {"OE_BACKFILTER", 800f },
        {"OE_EXITPATH", 800f },
        {"OE_JUNGLE05", 800f },
        {"OE_JUNGLE06", 800f },
        {"OE_CAVE10", 800f },
        {"OE_FINAL01", 800f },
        {"OE_CAVE11", 800f },
        {"OE_JUNGLEESCAPE", 800f },
        {"OE_CAVE09", 800f },
        {"OE_JUNGLE04", 800f },
        {"OE_CAVE04", 800f },
        {"OE_CAVE17", 800f },
        {"OE_CAVE18", 800f },
        {"OE_SEXTRA", 170f }
    };

    public static void ApplyWater()
    {
        On.RoomCamera.ChangeRoom += PlayerChangedRoomTrigger;
    }

    private static void PlayerChangedRoomTrigger(On.RoomCamera.orig_ChangeRoom orig, RoomCamera self, Room room, int camPos)
    {
        orig(self, room, camPos);
        var name = room.abstractRoom.name;
        if (/*!modifiedRooms.Contains(room) && */room.game.IsLeader() && swampRooms.ContainsKey(name))
        {
            if (room.waterObject == null)
            {
                room.AddWater();

            }
            modifiedRooms.Add(room);
            float height = room.waterObject.fWaterLevel;
            swampRooms.TryGetValue(name, out height);
            room.waterObject.fWaterLevel = height;
        }
    }
}