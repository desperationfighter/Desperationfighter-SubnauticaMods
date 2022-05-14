//for CustomClass and User Configsetting
using ModInstalLogger_BZ.Management;
//for List
using System.Collections.Generic;
//for File Operations
using System.IO;
//for Assembly info
using System.Reflection;
//for JSON Operation
using Newtonsoft.Json;
//for Logging
using MyLogger = QModManager.Utility;
//Time and Date
using System;

namespace ModInstalLogger_BZ.Patches
{
    class Gameboot
    {
        public static string GetModPath()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return path;
        }

        public static string GetModListFile()
        {
            string file = Path.Combine(GetModPath(), ModInstalLogger_BZ.Listfilename_Game);
            return file;
        }

        public static string GetPath_ModListChange_Added()
        {
            string file = Path.Combine(GetModPath(), ModInstalLogger_BZ.Listfilename_Game_Added);
            return file;
        }

        public static string GetPath_ModListChange_Removed()
        {
            string file = Path.Combine(GetModPath(), ModInstalLogger_BZ.Listfilename_Game_Removed);
            return file;
        }

        public static string GetPath_ModList_Userreadable()
        {
            string file = Path.Combine(Environment.CurrentDirectory, ModInstalLogger_BZ.Listfilename_Game_Userreadable);
            return file;
        }
        

        public static void Core_ModcheckforGame()
        {
            //Phase 0 - Get Currently running Mods
            List<Moddata> mymodlist = LoggerLogic.GetrunningMods();


            if (File.Exists(GetModListFile()))
            {
                //Phase 1.1 - Get Previous used Logs
                List<Moddata> ExistingModList = JsonConvert.DeserializeObject<List<Moddata>>(File.ReadAllText(GetModListFile()));
                //Phase 1.2 - Compare Logs
                LoggerLogic.ModCompare(ExistingModList, mymodlist, GetPath_ModListChange_Added(), GetPath_ModListChange_Removed(), "Gamewide");
            }
            else
            {
                LoggerLogic.ShowIngameMessage("No previous Mod List was found. Mod Compare will be done on next Gamestart.");
                MyLogger.Logger.Log(MyLogger.Logger.Level.Info, "No Previous File found. Skip Compare");
            }

            //Set Format for JSON File so its readable without Transforming for example with Notepad++ Plugin
            Formatting myformat = new Formatting();
            myformat = Formatting.Indented;
            string json = JsonConvert.SerializeObject(mymodlist, myformat);

            //write string to file
            try
            {
                //Phase 1.3 - Write Compare to File
                File.WriteAllText(GetModListFile(), json);
                MyLogger.Logger.Log(MyLogger.Logger.Level.Info, "Mod Compare List for Game was saved to Mod Folder");
            }
            catch
            {
                MyLogger.Logger.Log(MyLogger.Logger.Level.Error, "ErrorID:200 - Saving Compare List File failed");
            }


            if (ModInstalLogger_BZ.Config.WriteUserreadableList)
            {
                LoggerLogic.WriteReadableUserModList(GetPath_ModList_Userreadable(), mymodlist, "Gamewide");
            }

            
        }


    }
}
