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
using ChargerWirelessCharging.Item;
//
using ChargerWirelessCharging.Managment;
using System.IO;
using System;

namespace ChargerWirelessCharging
{
    [QModCore]
    public class ChargerWirelessCharging
    {
        internal static IngameConfigMenu Config { get; private set; }
        internal static TechType WirelessChargerChipTechType { get; private set; }
        internal static bool iknowwhatido { get; private set; }

        [QModPatch]
        public static void ChargerWirelessCharging_InitializationMethod()
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

            Logger.Log(Logger.Level.Debug, "ChargerWirelessCharging Initialization");

            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();

            var WirelessChargerChipBlueprint = new WirelessChargingChip();
            WirelessChargerChipBlueprint.Patch();
            WirelessChargerChipTechType = WirelessChargerChipBlueprint.TechType;

            Harmony harmony = new Harmony("ChargerWirelessCharging");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "ChargerWirelessCharging Patched");
        }
    }
}
