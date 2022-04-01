//for CustomClass and User Configsetting
using ModInstalLogger.Management;
//for List
using System.Collections.Generic;
//for File Operations
using System.IO;
//for Assembly info
using System.Reflection;
//for JSON Operation
using Oculus.Newtonsoft.Json;
//for Logging
using QModManager.Utility;

namespace ModInstalLogger.Patches
{
    class Gameboot
    {
        public static string GetModPath()
        {
            //string path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Subnautica\\IMOSL_Test.json";
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return path;
        }

        public static string GetModListFile()
        {
            string file = Path.Combine(GetModPath(), ModInstalLogger.Listfilename_Game);
            return file;
        }

        public static string GetPath_ModListChange_Added()
        {
            string file = Path.Combine(GetModPath(), ModInstalLogger.Listfilename_Game_Added);
            return file;
        }

        public static string GetPath_ModListChange_Removed()
        {
            string file = Path.Combine(GetModPath(), ModInstalLogger.Listfilename_Game_Removed);
            return file;
        }

        public static void Core_ModcheckforGame()
        {
            //Phase 1 - Get Currently running Mods
            List<Moddata> mymodlist = LoggerLogic.GetrunningMods();
            /*
            string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            List<datamodel> _data = new List<datamodel>();
            _data.Add(new datamodel()
            {
                Type = "SN",
                Date = timeStamp,
                Mods = mymodlist
            });
            */

            if (File.Exists(GetModListFile()))
            {
                //Phase 2 - Get Previous used Logs
                List<Moddata> ExistingModList = JsonConvert.DeserializeObject<List<Moddata>>(File.ReadAllText(GetModListFile()));
                //Phase 3 - Compare Logs
                LoggerLogic.ModCompare(ExistingModList, mymodlist, GetPath_ModListChange_Added(), GetPath_ModListChange_Removed(), "Gamewide");
            }
            else
            {
                Logger.Log(Logger.Level.Info, "No Previous File found. Skip Compare");
            }

            //Set Format for JSON File so its readable without Transforming for example with Notepad++ Plugin
            Formatting myformat = new Formatting();
            myformat = Formatting.Indented;
            string json = JsonConvert.SerializeObject(mymodlist, myformat);

            //write string to file
            try
            {
                //Phase 4 - Write Compare to File
                File.WriteAllText(GetModListFile(), json);
                Logger.Log(Logger.Level.Info, "Mod Compare List for Game was saved to Mod Folder");
            }
            catch
            {
                Logger.Log(Logger.Level.Error, "ErrorID:200 - Saving Compare List File failed");
            }
        }
    }
}
