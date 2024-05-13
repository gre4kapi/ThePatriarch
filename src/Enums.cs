using MoreSlugcats;
using static DataPearl.AbstractDataPearl;
namespace TheLeader;
public static class Enums
{
    public static SlugcatStats.Name Leader = new(nameof(Leader), false);
    public static bool registed = false;

    //public static MoreSlugcats.SSOracleRotBehavior.RMConversation MeetLeader;

    //Conversation.ID
    public static Conversation.ID Pebbles_Leader_FirstMeet;
    public static Conversation.ID Pebbles_Leader_AfterMet;
    public static void RegisterAllValues()
    {
        if (registed) return;

       //MeetLeader = new MoreSlugcats.SSOracleRotBehavior.RMConversation("MeetLeader", true);

        Pebbles_Leader_FirstMeet = new Conversation.ID("Pebbles_Leader_FirstMeet", true);
        Pebbles_Leader_AfterMet = new Conversation.ID("Pebbles_Leader_AfterMet", true);
        //GateRequirement.RegisterValues();
        registed = true;
    }

    public static void UnregisterAllValues()
    {
        if (registed)
        {
            //GateRequirement.UnregisterValues();

            Pebbles_Leader_FirstMeet.Unregister();
            Pebbles_Leader_FirstMeet = null;

            Pebbles_Leader_AfterMet.Unregister();
            Pebbles_Leader_AfterMet = null;
            //registed = false;
        }
    }
    public static DataPearlType FixedPebblesPearl = new(nameof(FixedPebblesPearl), false);
    public static RegionGate.GateRequirement LeaderLock = new RegionGate.GateRequirement("Leader", true);
    /*public static void Unregister<T>(ExtEnum<T> extEnum) where T : ExtEnum<T>
    {
        if (extEnum != null)
        {
            extEnum.Unregister();
        }
    }
    public class GateRequirement
    {
        public static void RegisterValues()
        {
            LeaderLock = new RegionGate.GateRequirement("Leader", true);
        }
        public static void UnregisterValues()
        {
            Enums.Unregister(LeaderLock);
        }
        public static RegionGate.GateRequirement LeaderLock;
    }*/
}