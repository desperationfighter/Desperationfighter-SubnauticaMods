using System;
//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API;
using QModManager.API.ModLoading;
//for Logging
using QModManager.Utility;
//For IngameConfig
using SMLHelper.V2.Handlers;
//
using CyclopsMultiDecoyLaunch.Management;
using SharedSNCode;
using System.IO;

namespace CyclopsMultiDecoyLaunch
{
    [QModCore]
    public static class CyclopsMultiDecoyLaunch
    {
        internal static Assembly assembly = Assembly.GetExecutingAssembly();
        internal static IngameConfigMenu Config { get; private set; }
        internal static bool iknowwhatido { get; private set; }

        [QModPatch]
        public static void CyclopsMultiDecoyLaunch_InitializationMethod()
        {
             Logger.Log(Logger.Level.Debug, $"{assembly.GetName().Name} Initialization");
            Harmony harmony = new Harmony(assembly.GetName().Name);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            //Add the Ingame Config for User
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();
            Logger.Log(Logger.Level.Info, $"{assembly.GetName().Name} Patched");

            //iknowwhatido = true;
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, "iknowwhatido.override")))
            {
                iknowwhatido = true;
            }
            else
            {
                //if (business.itsok && QModServices.Main.PirateDetected)
                if (business.itsok)
                {
                    iknowwhatido = true;
                }
                else
                {
                    iknowwhatido = false;
                }
            }

            Logger.Log(Logger.Level.Debug, $"test start");
            foreach (var entry in Equipment.slotMapping)
            {
                Logger.Log(Logger.Level.Debug, $"{entry.Key} - {entry.Value.ToString()}");
            }
            Equipment.slotMapping.Add("DecoySlot6", EquipmentType.DecoySlot);
            Equipment.slotMapping.Add("DecoySlot7", EquipmentType.DecoySlot);
            Equipment.slotMapping.Add("DecoySlot8", EquipmentType.DecoySlot);
            Logger.Log(Logger.Level.Debug, $"after adding");
            foreach (var entry in Equipment.slotMapping)
            {
                Logger.Log(Logger.Level.Debug, $"{entry.Key} - {entry.Value.ToString()}");
            }
        }
    }
}