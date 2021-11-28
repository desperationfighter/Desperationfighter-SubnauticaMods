//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//for Logging
using QModManager.Utility;

namespace Warnbeforebreak_BZ
{
    [QModCore]
    public static class Warnbeforebreak_BZ
    {
        [QModPatch]
        public static void warnbeforebreak_InitializationMethod()
        {
            Logger.Log(Logger.Level.Debug, "Warnbeforebreak_BZ Initialization");
            Harmony harmony = new Harmony("Warnbeforebreak_BZ");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "Warnbeforebreak_BZ Patched");
        }
    }
}
