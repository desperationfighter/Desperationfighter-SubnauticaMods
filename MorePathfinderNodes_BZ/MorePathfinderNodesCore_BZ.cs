using HarmonyLib;
using QModManager.API.ModLoading;
using QModManager.Utility;
using SMLHelper.V2.Handlers;
using System.Reflection;
using MorePathfinderNodes_BZ.Managment;

namespace MorePathfinderNodes_BZ
{
    [QModCore]
    public class MorePathfinderNodesCore_BZ
    {
        internal static IngameConfigMenu Config { get; private set; }

        [QModPatch]
        public static void InitializationMethod()
        {
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();

            Logger.Log(Logger.Level.Debug, "MorePathfinderNodes_BZ Initialization");
            Harmony harmony = new Harmony("MorePathfinderNodes_BZ");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "MorePathfinderNodes_BZ Patched");

            //QModServices.Main.AddCriticalMessage("Warning the MorePathfinderNodes_BZ Mod is in BETA Status !");
        }
    }
}