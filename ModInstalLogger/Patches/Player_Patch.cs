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

namespace ModInstalLogger.Patches
{
    internal class Player_Patch
    {
    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch(nameof(Player.Awake))]
    public static class Player_Awake_Patch
    {
        [HarmonyPostfix]
        public static void Postfix()
        {
            //Application.persistentDataPath
            //Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //QModServices.Main.AddCriticalMessage($"TSTFINDER200: {SaveUtils.GetCurrentSaveDataDir()} + {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
            //QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, $"TSTFINDER200: {SaveUtils.GetCurrentSaveDataDir()}  + {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");

            string CurrentSavegameDatadir = SaveUtils.GetCurrentSaveDataDir();

            string tmppath = Path.Combine(CurrentSavegameDatadir, ModInstalLogger.Listfilename_Savegame);

            List<Moddata> mymodlist = LoggerLogic.GetrunningMods();

            if (File.Exists(tmppath))
            {
                List<Moddata> ExistingModList = JsonConvert.DeserializeObject<List<Moddata>>(File.ReadAllText(tmppath));
                LoggerLogic.ModCompare(ExistingModList, mymodlist, GetPath_SavegameModListChange_Added(CurrentSavegameDatadir), GetPath_SavegameModListChange_Removed(CurrentSavegameDatadir), "Savegame");
            }

            Formatting myformat = new Formatting();
            myformat = Formatting.Indented;
            string json = JsonConvert.SerializeObject(mymodlist, myformat);

            //write string to file
            System.IO.File.WriteAllText(tmppath, json);
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
