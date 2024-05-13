using CustomSaveTx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TheLeader;

public class PearlWritedSave : DeathPersistentSaveDataTx
{
    public static bool pearlWrited;

    public override string header
    {
        get
        {
            return "LEADERPEARLWRITED";
        }
    }

    public PearlWritedSave(SlugcatStats.Name name) : base(name)
    {
        this.slugName = name;
    }

    public override string SaveToString(bool saveAsIfPlayerDied, bool saveAsIfPlayerQuit)
    {
        string result;
        if (saveAsIfPlayerDied || saveAsIfPlayerQuit)
        {
            result = this.origSaveData;
        }
        else
        {
            result = pearlWrited.ToString();
        }

        return result;
    }

    public override void LoadDatas(string data)
    {
        base.LoadDatas(data);
        pearlWrited = bool.Parse(data);
    }

    public override void ClearDataForNewSaveState(SlugcatStats.Name newSlugName)
    {
        base.ClearDataForNewSaveState(newSlugName);
        if (pearlWrited)
        {
            pearlWrited = false;
        }
    }
}
