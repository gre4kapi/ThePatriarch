
using System.Collections.Generic;
using UnityEngine;

namespace ThePatriarch;

public class SaveMiscProgression
{
    // Meta
    public bool IsNewPearlcatSave { get; set; } = true;
    public bool IsMSCSave { get; set; } = ModManager.MSC;

    // Story

    public bool IsPearlpupSick { get; set; }
    public bool HasOEEnding { get; set; }
    public bool JustAscended { get; set; }
    public bool Ascended { get; set; }
    public bool HasTrueEnding { get; set; }


    public void ResetSave()
    {

        IsNewPearlcatSave = true;

        HasOEEnding = false;

        JustAscended = false;
        Ascended = false;
        HasTrueEnding = false;

        IsMSCSave = ModManager.MSC;
    }
}