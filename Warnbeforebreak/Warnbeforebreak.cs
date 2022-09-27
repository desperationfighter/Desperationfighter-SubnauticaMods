//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//for Logging
using QModManager.Utility;

namespace Warnbeforebreak
{
    [QModCore]
    public static class Warnbeforebreak
    {
        [QModPatch]
        public static void warnbeforebreak_InitializationMethod()
        {
            Logger.Log(Logger.Level.Debug, "Warnbeforebreak Initialization");
            Harmony harmony = new Harmony("Warnbeforebreak");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "Warnbeforebreak Patched");
        }
    }
}