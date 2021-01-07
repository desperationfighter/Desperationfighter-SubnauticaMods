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

//working space
using SMLHelper.V2.Handlers;
using CyclopsEngineOverheatMonitor.Management;

namespace CyclopsEngineOverheatMonitor
{
    [QModCore]
    public class CyclopsEngineOverheat
    {
        internal static CyclopsEngineOverheatConfigIngameMenu Config { get; private set; }
        
        [QModPatch]
        public static void Patch()
        {
            Logger.Log(Logger.Level.Debug, "Cyclops Engine Overheat Moniter by desperationfighter start patching");

            //Patch the Mod itself
            Harmony harmony = new Harmony("CyclopsEngineOverheat");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            //Add the Ingame Config for User
            Config = OptionsPanelHandler.Main.RegisterModOptions<CyclopsEngineOverheatConfigIngameMenu>();
            
            Logger.Log(Logger.Level.Info, "Patched successfully!");
        }
    }
}
