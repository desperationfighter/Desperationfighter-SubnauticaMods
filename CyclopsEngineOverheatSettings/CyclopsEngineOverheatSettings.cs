﻿//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//for Logging
using QModManager.Utility;
//working space
using SMLHelper.V2.Handlers;
using CyclopsEngineOverheatMonitor.Management;

namespace CyclopsEngineOverheatMonitor
{
    [QModCore]
    public class CyclopsEngineOverheatSettings
    {
        internal static CyclopsEngineOverheatSettings_ConfigIngameMenu Config { get; private set; }

        [QModPatch]
        public static void Patch()
        {
            Logger.Log(Logger.Level.Debug, "Cyclops Engine Overheat Settings start patching");

            //Patch the Mod itself
            Harmony harmony = new Harmony("CyclopsEngineOverheatSettings");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            //Add the Ingame Config for User
            Config = OptionsPanelHandler.Main.RegisterModOptions<CyclopsEngineOverheatSettings_ConfigIngameMenu>();

            Logger.Log(Logger.Level.Info, "Patched successfully!");
        }
    }
}