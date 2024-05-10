using System;
using System.Collections.Generic;
using BepInEx.Logging;
using RWCustom;
using UnityEngine;

namespace TheLeader;
public partial class Hooks
{
    private static readonly List<Room> modifiedRooms = new List<Room>();

    public static void ApplyWater()
    {
        On.RoomCamera.ChangeRoom += PlayerChangedRoomTrigger;
    }


    private static void PlayerChangedRoomTrigger(On.RoomCamera.orig_ChangeRoom orig, RoomCamera self, Room room, int camPos)
    {
        orig(self, room, camPos);
        if (!modifiedRooms.Contains(room))
        {
            modifiedRooms.Add(room);
        }
        if (room.waterObject == null)
        {
            room.AddWater();

        }
        room.waterObject.fWaterLevel = room.MiddleOfTile(new IntVector2(0, (int)(room.Height * UnityEngine.Random.Range(0.2f, 0.6f)))).y;
        var message = "fWater: "+ room.waterObject.fWaterLevel;
        Debug.Log(message);
    }
}