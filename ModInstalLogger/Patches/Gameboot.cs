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
using MyLogger = QModManager.Utility;
//Building Logfile
using System.Text;
//Time and Date
using System;

namespace ModInstalLogger.Patches
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

        public static string GetPath_ModList_Userreadable()
        {
            string file = Path.Combine(Environment.CurrentDirectory, ModInstalLogger.Listfilename_Game_Userreadable);
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


            if (ModInstalLogger.Config.WriteUserreadableList)
            {
                //Phase 2.1 - Init 
                StringBuilder stringBuilder = new StringBuilder();
                string timeStamp = DateTime.Now.ToString("yyyy.MM.dd - HH:mm");
                stringBuilder.AppendLine("Mod Instal Logger - Modlist Export");
                stringBuilder.AppendLine($"This Export was created at {timeStamp}");
                stringBuilder.AppendLine($"- - - --- - - - --- --- - - - --- - - - --- --- - - - --- - - -");


                if(!business.itsok())
                {
                    stringBuilder.AppendLine($"- - - --- - - - --- --- - - - --- - - - --- --- - - - --- - - -");
                    stringBuilder.AppendLine($"] - - - --- - - - --- <<< ||| \\\\\\ /// ||| >>> --- - - - --- - - -[");
                    stringBuilder.AppendLine($"- - - --- - - - --- --- - - - --- - - - --- --- - - - --- - - -");
                    stringBuilder.AppendLine($"Thank you for using my Mod and enjoying all the Other Mods.");
                    stringBuilder.AppendLine($"If you like the Game that much you already Mod it,");
                    stringBuilder.AppendLine($"may think about buying it. Pirating a Game is not a nice thing.");
                    stringBuilder.AppendLine($"- - - --- - - - --- --- - - - --- - - - --- --- - - - --- - - -");
                    stringBuilder.AppendLine($"] - - - --- - - - --- <<< ||| /// \\\\\\ ||| >>> --- - - - --- - - - [");
                    stringBuilder.AppendLine($"- - - --- - - - --- --- - - - --- - - - --- --- - - - --- - - -");
                }

                stringBuilder.AppendLine($"Mod Counter ; Mod Display Name ; Mod Author ; Mod Version ; Mod Internal ID");
                stringBuilder.AppendLine($"- - - --- - - - --- --- - - - --- - - - --- --- - - - --- - - -");

                List<Moddata> enabledmods = new List<Moddata>();
                List<Moddata> disabledmods = new List<Moddata>();
                foreach (Moddata moddata in mymodlist)
                {
                    if (moddata.Enabled)
                    {
                        enabledmods.Add(moddata);
                    }
                    else
                    {
                        disabledmods.Add(moddata);
                    }
                }

                //Phase 2.2 - Write active Mods
                stringBuilder.AppendLine("> Active Mods:");
                if (enabledmods.Count == 0)
                {
                    stringBuilder.AppendLine($"There are no Active Mods running currently");
                }
                else
                {
                    int counter = 1;
                    foreach (Moddata moddata in enabledmods)
                    {
                        stringBuilder.AppendLine($"Mod {counter.ToString("000")}; {moddata.Displayname} ; {moddata.Author} ; {moddata.Version}");
                        counter++;
                    }
                }

                if (ModInstalLogger.Config.WriteUserreadableList_includedisabled)
                {
                    //Phase 2.3 - Write discabled Mods
                    stringBuilder.AppendLine("> Disabled Mods:");
                    if (disabledmods.Count == 0)
                    {
                        stringBuilder.AppendLine($"There are no Disabled Mods");
                    }
                    else
                    {
                        int counter_disabled = 1;
                        foreach (Moddata moddata in disabledmods)
                        {
                            stringBuilder.AppendLine($"Mod {counter_disabled.ToString("000")}; {moddata.Displayname} ; {moddata.Author} ; No Version available");
                            counter_disabled++;
                        }
                    }
                }

                //write string to file
                try
                {
                    //Phase 2.4 - Write Compare to File
                    File.WriteAllText(GetPath_ModList_Userreadable(), stringBuilder.ToString());
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Info, "User Readable Modlist was saved to Game Folder.");
                }
                catch
                {
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Error, "ErrorID:202 - Saving User Readable Modlist File failed");
                }
            }
        }
    }
}
