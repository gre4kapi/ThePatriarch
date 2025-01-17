using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ThePatriarch;
partial class Hooks
{

    public static void SetupOracles()
    {
        On.Oracle.ctor += OracleCtor;
        ApplyPebbles();
        ApplyMoon();
    }
    public static void OracleCtor(On.Oracle.orig_ctor orig, Oracle self, AbstractPhysicalObject abstractPhysicalObject, Room room)
    {
        orig(self, abstractPhysicalObject, room);
        OracleCWT.Add(self, new StrongBox<int>(0));
    }
    public static string Translate(String s)
    {
        return RWCustom.Custom.rainWorld.inGameTranslator.Translate(s);
    }
}
