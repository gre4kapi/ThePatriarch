using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using MoreSlugcats;

namespace ThePatriarch;
partial class Hooks
{
    public static Vector2 playerPos;
    public static void ApplyPlayerHooks()
    {
        On.Player.GraspsCanBeCrafted += Player_GraspsCanBeCrafted;
        On.Player.Update += PlayerUpdate;
        On.Player.Grabability += Player_DualWield;
        On.Player.CanIPickThisUp += Player_DislodgeSpear;
    }

    private static bool Player_DislodgeSpear(On.Player.orig_CanIPickThisUp orig, Player self, PhysicalObject obj)
    {
        if (obj is Weapon && ((ExtEnum<Weapon.Mode>)(obj as Weapon).mode == (ExtEnum<Weapon.Mode>)Weapon.Mode.StuckInWall && self.room.game.IsPatriarch()))
            return true;
        return orig(self, obj);
    }

    private static bool Player_GraspsCanBeCrafted(On.Player.orig_GraspsCanBeCrafted orig, Player self)
    {
        if (self.slugcatStats.name == Enums.Patriarch && (self.CraftingResults() != null))
        {
            return true;
        }
        return orig(self);
    }
    public static void PlayerUpdate(On.Player.orig_Update orig, Player player, bool eu)
    {
        orig(player, eu);
        playerPos = player.mainBodyChunk.pos;
    }
    public static Player.ObjectGrabability Player_DualWield(On.Player.orig_Grabability orig, Player self, PhysicalObject obj)
    {
        if (obj is Weapon)
        {
            if (self.room.game.IsPatriarch())
            {
                return Player.ObjectGrabability.OneHand;
            }
        }
        return orig(self, obj);
    }
}