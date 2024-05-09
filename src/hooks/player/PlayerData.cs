namespace TheLeader;
public static partial class Hooks
{
    public static bool StoryCharacter(this RainWorldGame? game) => game?.StoryCharacter == Enums.Leader;
}