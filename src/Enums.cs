using MoreSlugcats;
namespace TheLeader;
public static class Enums
{
    public static SlugcatStats.Name Leader = new(nameof(Leader), false);
    public static bool registed = false;

    //public static MoreSlugcats.SSOracleRotBehavior.RMConversation MeetLeader;

    //Conversation.ID
    public static Conversation.ID Pebbles_Leader_FirstMeet;
    public static Conversation.ID Pebbles_Leader_AfterMet;
    public static void RegisterValues()
    {
        if (registed) return;

       //MeetLeader = new MoreSlugcats.SSOracleRotBehavior.RMConversation("MeetLeader", true);

        Pebbles_Leader_FirstMeet = new Conversation.ID("Pebbles_Leader_FirstMeet", true);
        Pebbles_Leader_AfterMet = new Conversation.ID("Pebbles_Leader_AfterMet", true);
        registed = true;
    }

    public static void UnregisterValues()
    {
        if (registed)
        {

            Pebbles_Leader_FirstMeet.Unregister();
            Pebbles_Leader_FirstMeet = null;

            Pebbles_Leader_AfterMet.Unregister();
            Pebbles_Leader_AfterMet = null;
        }
    }
}