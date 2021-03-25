using HarmonyLib;
using QModManager.API.ModLoading;
using QModManager.API;
using QModManager.Utility;
using SMLHelper.V2.Handlers;
using System.Reflection;
using MorePathfinderNodes.Managment;

namespace MorePathfinderNodes
{
    [QModCore]
    public class MorePathfinderNodesCore
    {
        internal static IngameConfigMenu Config { get; private set; }

        [QModPatch]
        public static void InitializationMethod()
        {
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();

            Logger.Log(Logger.Level.Debug, "MorePathfinderNodes Initialization");
            Harmony harmony = new Harmony("MorePathfinderNodes");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "MorePathfinderNodes Patched");

            //QModServices.Main.AddCriticalMessage("Warning the MorePathfinderNodes Mod is in BETA Status !");
        }
    }
}
