using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
using MoreCyclopsUpgrades.API;
using SMLHelper.V2.Handlers;
using CyclopsEngineOverheatMonitor.Items;
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

            Harmony harmony = new Harmony("CyclopsEngineOverheat");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Config = OptionsPanelHandler.Main.RegisterModOptions<CyclopsEngineOverheatConfigIngameMenu>();
            Logger.Log(Logger.Level.Info, "Patched successfully!");

        //var OverheadMonitor = new CylopsEngineOverheatMonitorModule();
        //OverheadMonitor.Patch();

        /*
        return new UpgradeHandler(TechType.CyclopsOverheatMonitorModule, cyclops)
        {
            OnClearUpgrades = () => { cyclops.shieldUpgrade = false; },
            OnUpgradeCounted = () => { cyclops.shieldUpgrade = true; }
        };
        */
        QModServices.Main.AddCriticalMessage("The Cyclops mod was loaded");
        }
    }
}
