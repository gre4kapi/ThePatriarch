using MoreSlugcats;
using static DataPearl.AbstractDataPearl;
using static Menu.MenuScene;
using static Menu.SlideShow;
namespace ThePatriarch;

public static class Enums
{
    public static SlugcatStats.Name Patriarch = new(nameof(Patriarch), false);
    public static bool registed = false;

    //public static MoreSlugcats.SSOracleRotBehavior.RMConversation MeetPatriarch;

    //Conversation.ID
    public static Conversation.ID Pebbles_Patriarch_FirstMeet;
    public static Conversation.ID Pebbles_Patriarch_AfterMet;
    public static void RegisterAllValues()
    {
        if (registed) return;

       //MeetPatriarch = new MoreSlugcats.SSOracleRotBehavior.RMConversation("MeetPatriarch", true);

        Pebbles_Patriarch_FirstMeet = new Conversation.ID("Pebbles_Patriarch_FirstMeet", true);
        Pebbles_Patriarch_AfterMet = new Conversation.ID("Pebbles_Patriarch_AfterMet", true);
        //GateRequirement.RegisterValues();
        registed = true;
    }

    public static void UnregisterAllValues()
    {
        if (registed)
        {
            //GateRequirement.UnregisterValues();

            Pebbles_Patriarch_FirstMeet.Unregister();
            Pebbles_Patriarch_FirstMeet = null;

            Pebbles_Patriarch_AfterMet.Unregister();
            Pebbles_Patriarch_AfterMet = null;
            //registed = false;
        }
    }
    public static DataPearlType FixedPebblesPearl = new(nameof(FixedPebblesPearl), false);
    public static RegionGate.GateRequirement PatriarchLock = new RegionGate.GateRequirement("Patriarch", true);
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
            PatriarchLock = new RegionGate.GateRequirement("Patriarch", true);
        }
        public static void UnregisterValues()
        {
            Enums.Unregister(PatriarchLock);
        }
        public static RegionGate.GateRequirement PatriarchLock;
    }*/
    public static class Scenes
    {
        public static SceneID Dream_Patriarch_Random = new(nameof(Dream_Patriarch_Random), false);
    }
    public static class Dreams
    {
        public static DreamsState.DreamID Dream_Patriarch_Random = new(nameof(Dream_Patriarch_Random), true);

        public static void RegisterDreams()
        {
            SlugBase.Assets.CustomDreams.SetDreamScene(Dream_Patriarch_Random, Scenes.Dream_Patriarch_Random);
        }
    }
}