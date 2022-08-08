//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//for Logging
using QModManager.Utility;
//For IngameConfig
using SMLHelper.V2.Handlers;
//For item
using CargerWirelessCharging.Item;
//
using CargerWirelessCharging.Managment;
using System.IO;
using System;

namespace CargerWirelessCharging
{
    [QModCore]
    public class CargerWirelessCharging
    {
        internal static IngameConfigMenu Config { get; private set; }
        internal static TechType WirelessChargerChipTechType { get; private set; }
        internal static bool iknowwhatido { get; private set; }

        [QModPatch]
        public static void CargerWirelessCharging_InitializationMethod()
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "iknowwhatido.override")))
            {
                iknowwhatido = true;
            }
            else
            {
                /*
                if (business.itsok && QModServices.Main.PirateDetected)
                {
                    iknowwhatido = true;
                }
                else
                {
                    iknowwhatido = false;
                }
                */
            }

            Logger.Log(Logger.Level.Debug, "CargerWirelessCharging Initialization");

            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();

            var WirelessChargerChipBlueprint = new WirelessChargingChip();
            WirelessChargerChipBlueprint.Patch();
            WirelessChargerChipTechType = WirelessChargerChipBlueprint.TechType;

            Harmony harmony = new Harmony("CargerWirelessCharging");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "CargerWirelessCharging Patched");
        }
    }
}
