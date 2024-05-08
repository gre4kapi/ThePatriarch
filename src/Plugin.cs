using System;
using BepInEx;
using UnityEngine;
using SlugBase.Features;
using static SlugBase.Features.FeatureTypes;
using UnityEngine.Diagnostics;
using MoreSlugcats;

namespace SlugTemplate
{
    [BepInPlugin(MOD_ID, "The Leader", "0.1.0")]
    class Plugin : BaseUnityPlugin
    {
        private const string MOD_ID = "gre4ka.theleader";

        public static readonly PlayerFeature<float> SuperJump = PlayerFloat("theleader/super_jump");
        public static readonly PlayerFeature<bool> ExplodeOnDeath = PlayerBool("theleader/explode_on_death");
        public static readonly GameFeature<float> MeanLizards = GameFloat("theleader/mean_lizards");


        // Add hooks
        public void OnEnable()
        {
            On.RainWorld.OnModsInit += Extras.WrapInit(LoadResources);

            // Put your custom hooks here!
            On.MoreSlugcats.MSCRoomSpecificScript.OE_GourmandEnding.Update += OE_GourmandEnding_Update;
        }
        
        // Load any resources, such as sprites or sounds
        private void LoadResources(RainWorld rainWorld)
        {
        }

        private static void OE_GourmandEnding_Update(On.MoreSlugcats.MSCRoomSpecificScript.OE_GourmandEnding.orig_Update orig, MSCRoomSpecificScript.OE_GourmandEnding self, bool eu)
        {
            return;
            orig(self, eu);
        }
    }
}