using System;
using System.Linq;
using UnityEngine;

namespace TheLeader;

public static partial class Hooks
{
    public static void ApplyHooks()
    {
        ApplyWorldHooks();
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
            ApplyHooks();

            // Init Enums
            _ = Enums.Leader;
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
