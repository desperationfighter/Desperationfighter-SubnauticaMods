//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//to show a test
using QModManager.API;
//for Logging
using QModManager.Utility;

namespace DAATQS
{
    [QModCore]
    public static class DAATQS_harmony
    {
        [QModPatch]
        public static void DAATQS_InitializationMethod()
        {
            Logger.Log(Logger.Level.Debug, "DAATQS Initialization");

            Harmony harmony = new Harmony("DAATQS");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Logger.Log(Logger.Level.Info, "DAATQS Patched");

            // QModServices.Main.AddCriticalMessage("Warning the DAATQS Mod is in BETA Status !");
        }
    }
}
