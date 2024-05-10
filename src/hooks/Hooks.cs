using System;
using System.Linq;
using UnityEngine;

namespace TheLeader;

public static partial class Hooks
{
    public static void ApplyHooks()
    {
        ApplyWorldHooks();
        ApplyConvs();
        ApplyWater();
    }

    public static void ApplyInit()
    {
        On.RainWorld.OnModsInit += RainWorld_OnModsInit;
    }

    public static bool IsInit { get; private set; } = false;

    private static void RainWorld_OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        try
        {

            if (IsInit) return;
            IsInit = true;
            //Leader = new(nameof(Leader), true);
            ApplyHooks();

            // Init Enums
            _ = Enums.Leader;
            Enums.RegisterValues();
        }
        catch (Exception e)
        {
        }
        finally
        {
            orig(self);
        }
    }
}
