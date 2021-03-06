﻿//for Assembly info
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
using DAATQS.Managment;

namespace DAATQS
{
    [QModCore]
    public static class DAATQS_harmony
    {
        internal static IngameConfigMenu Config { get; private set; }
        internal static TechTypeAllowList allowList { get; private set; }

        [QModPatch]
        public static void DAATQS_InitializationMethod()
        {
            Logger.Log(Logger.Level.Debug, "DAATQS Initialization");

            Harmony harmony = new Harmony("DAATQS");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            //Add the Ingame Config for User
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();
            allowList = OptionsPanelHandler.Main.RegisterModOptions<TechTypeAllowList>();

            Logger.Log(Logger.Level.Info, "DAATQS Patched");

            // QModServices.Main.AddCriticalMessage("Warning the DAATQS Mod is in BETA Status !");
        }
    }
}
