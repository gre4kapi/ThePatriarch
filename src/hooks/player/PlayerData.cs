namespace ThePatriarch;
public static partial class Hooks
{
    public static bool IsPatriarch(this RainWorldGame? game) => game?.StoryCharacter == Enums.Patriarch;
    public static void ApplyPlayerHooks()
    {
        On.Player.GraspsCanBeCrafted += Player_GraspsCanBeCrafted;
    }
    private static bool Player_GraspsCanBeCrafted(On.Player.orig_GraspsCanBeCrafted orig, Player self)
    {
        if (self.slugcatStats.name == Enums.Patriarch && (self.CraftingResults() != null))
        {
            return true;
        }
        return orig(self);
    }
}