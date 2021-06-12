//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//for Logging
using QModManager.Utility;
using SMLHelper.V2.Handlers;
using BetterThumper.Managment;

namespace BetterThumper
{
    [QModCore]
    public class BetterThumper
    {
        internal static IngameConfigMenu Config { get; private set; }

        [QModPatch]
        public static void BetterThumper_Initial()
        {
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();

            Logger.Log(Logger.Level.Debug, "BetterThumper Initialization");

            Harmony harmony = new Harmony("BetterThumper");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Logger.Log(Logger.Level.Info, "BetterThumper Patched");

            //QModServices.Main.AddCriticalMessage("Warning the BetterThumper Mod is in BETA Status !");

        }
    }
}
