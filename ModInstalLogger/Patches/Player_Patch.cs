//for CustomClass and User Configsetting
using ModInstalLogger.Management;
//for Patching Game Class
using HarmonyLib;
//for JSON Operation
using Oculus.Newtonsoft.Json;
//for File Operations
using System.IO;
//for getting Savegame infos from Game
using SMLHelper.V2.Utility;
//for List
using System.Collections.Generic;
//for Logging
using MyLogger = QModManager.Utility;

namespace ModInstalLogger.Patches
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch(nameof(Player.Awake))]
    public static class Player_Awake_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            //Get Current Savegame Slot
            string CurrentSavegameDatadir = SaveUtils.GetCurrentSaveDataDir();
            //Create Full Path to Compare File
            string tmppath = Path.Combine(CurrentSavegameDatadir, ModInstalLogger.Listfilename_Savegame);

            //Phase 1 - Get Currently running Mods
            List<Moddata> mymodlist = LoggerLogic.GetrunningMods();

            if (File.Exists(tmppath))
            {
                //Phase 2 - Get Previous used Logs
                List<Moddata> ExistingModList = JsonConvert.DeserializeObject<List<Moddata>>(File.ReadAllText(tmppath));
                //Phase 3 - Compare Logs
                LoggerLogic.ModCompare(ExistingModList, mymodlist, GetPath_SavegameModListChange_Added(CurrentSavegameDatadir), GetPath_SavegameModListChange_Removed(CurrentSavegameDatadir), "Savegame");
            }
            else
            {
                MyLogger.Logger.Log(MyLogger.Logger.Level.Info, "No Previous File found. Skip Compare");
            }

            //Set Format for JSON File so its readable without Transforming for example with Notepad++ Plugin
            Formatting myformat = new Formatting();
            myformat = Formatting.Indented;
            string json = JsonConvert.SerializeObject(mymodlist, myformat);

            //write string to file
            try
            {
                //Phase 4 - Write Compare to File
                File.WriteAllText(tmppath, json);
                MyLogger.Logger.Log(MyLogger.Logger.Level.Info, "Mod Compare List for Savegame was saved to Temp Savegame Folder");
            }
            catch
            {
                MyLogger.Logger.Log(MyLogger.Logger.Level.Error, "ErrorID:500 - Saving Compare List File failed");
            }

            if (ModInstalLogger.Config.WriteUserreadableList)
            {
                string savepath = Path.Combine(CurrentSavegameDatadir, ModInstalLogger.Listfilename_Savegame_Userreadable);
                LoggerLogic.WriteReadableUserModList(savepath, mymodlist, "Savegame");
            }
        }

        public static string GetPath_SavegameModListChange_Added(string path)
        {
            string file = Path.Combine(path, ModInstalLogger.Listfilename_Savegame_Added);
            return file;
        }

        public static string GetPath_SavegameModListChange_Removed(string path)
        {
            string file = Path.Combine(path, ModInstalLogger.Listfilename_Savegame_Removed);
            return file;
        }
    }
}
