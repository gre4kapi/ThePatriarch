using System;
using BepInEx;
using BepInEx.Logging;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Security;
using UnityEngine;
using static ThePatriarch.Hooks;
using UnityEngine.Diagnostics;
using MoreSlugcats;
using System.Drawing.Text;
using System.Runtime.CompilerServices;

namespace ThePatriarch
{
    [BepInPlugin(MOD_ID, "The Patriarch", "0.1.0")]
    class Plugin : BaseUnityPlugin
    {
        public const string MOD_ID = "gre4ka.thepatriarch";
        public const string CAT_NAME = "Patriarch";
        static public SlugcatStats.Name SlugName;
        //public static DataPearl.AbstractDataPearl.DataPearlType FixedPebblesPearl;

        //public new static ManualLogSource Logger { get; private set; } = null!;
        public static bool gateLock = true;

        // Add hooks
        public void OnEnable()
        {
            //Logger = base.Logger;
            ApplyInit();

        }

        // Load any resources, such as sprites or sounds
        private void LoadResources(RainWorld rainWorld)
        {
        }
    }
}