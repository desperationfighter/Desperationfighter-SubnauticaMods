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
//For IngameConfig
using SMLHelper.V2.Handlers;
//working space
using DAATQS_BZ.Managment;

namespace DAATQS_BZ
{
    [QModCore]
    public static class DAATQS_BZ_harmony
    {
        internal static IngameConfigMenu Config { get; private set; }
        internal static TechTypeAllowList allowList { get; private set; }

        [QModPatch]
        public static void DAATQS_BZ_InitializationMethod()
        {
            Logger.Log(Logger.Level.Debug, "DAATQS_BZ Initialization");

            Harmony harmony = new Harmony("DAATQS_BZ");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            //Add the Ingame Config for User
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();
            allowList = OptionsPanelHandler.Main.RegisterModOptions<TechTypeAllowList>();

            Logger.Log(Logger.Level.Info, "DAATQS_BZ Patched");

            // QModServices.Main.AddCriticalMessage("Warning the DAATQS Mod is in BETA Status !");
        }
    }
}