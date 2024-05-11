namespace TheLeader;
public static partial class Hooks
{
    public static bool IsLeader(this RainWorldGame? game) => game?.StoryCharacter == Enums.Leader;
}