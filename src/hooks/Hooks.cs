using System;
using System.Linq;
using UnityEngine;
using ThePatriarch.effects;
using CustomSaveTx;

namespace ThePatriarch;

public static partial class Hooks
{
    public static void ApplyHooks()
    {
        ApplyWorldHooks();
        ApplyPlayerHooks();
        SetupOracles();
        ApplyWater();
        ApplySpawnHook();
        ApplyPearlHook();
        GateInit();
        WritedDataPearlEffect.HooksOn();
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
            //Patriarch = new(nameof(Patriarch), true);
            ApplyHooks();

            // Init Enums
            _ = Enums.Patriarch;
            Enums.RegisterAllValues();
            DeathPersistentSaveDataRx.AppplyTreatment(new PearlWritedSave(Plugin.SlugName));
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
