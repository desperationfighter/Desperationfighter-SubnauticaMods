﻿//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//for Logging
using QModManager.Utility;
//for IngameConfigMenu;
using ModInstalLogger.Management;
//for Game side
using ModInstalLogger.Patches;
//For IngameConfig
using SMLHelper.V2.Handlers;

namespace ModInstalLogger
{
    [QModCore]
    public class ModInstalLogger
    {
        internal static IngameConfigMenu Config { get; private set; }

        internal static string Listfilename_Game { get { return "MIL_Game.json"; } }
        internal static string Listfilename_Game_Removed { get { return "MIL_Game_Removed.json"; } }
        internal static string Listfilename_Game_Added { get { return "MIL_Game_Added.json"; } }
        internal static string Listfilename_Savegame { get { return "MIL_Savegame.json"; } }
        internal static string Listfilename_Savegame_Removed { get { return "MIL_Savegame_Removed.json"; } }
        internal static string Listfilename_Savegame_Added { get { return "MIL_Savegame_Added.json"; } }


        [QModPatch]
        public static void Initial()
        {
            Logger.Log(Logger.Level.Debug, "ModInstalLogger Initialization");
            Harmony harmony = new Harmony("desp_ModInstalLogger");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "ModInstalLogger Patched");

            //Add the Ingame Config for User
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();
        }

        [QModPostPatch]
        public static void Post()
        {
            Gameboot.Core_ModcheckforGame();
        }
    }
}
