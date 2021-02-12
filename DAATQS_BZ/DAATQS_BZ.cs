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

namespace DAATQS_BZ
{
    [QModCore]
    public static class DAATQS_BZ_harmony
    {
        [QModPatch]
        public static void DAATQS_BZ_InitializationMethod()
        {
            Logger.Log(Logger.Level.Debug, "DAATQS_BZ Initialization");

            Harmony harmony = new Harmony("DAATQS_BZ");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Logger.Log(Logger.Level.Info, "DAATQS_BZ Patched");

            // QModServices.Main.AddCriticalMessage("Warning the DAATQS Mod is in BETA Status !");
        }
    }
}