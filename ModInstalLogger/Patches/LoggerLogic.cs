//for Console Output
using System;
//for File Operations
using System.IO;
//for calling isntalled Mods
using QModManager.API;
//for Logging
using MyLogger = QModManager.Utility;
//for List
using System.Collections.Generic;
//for CustomClass and User Configsetting
using ModInstalLogger.Management;
//for JSON Formatting
using Oculus.Newtonsoft.Json;
//Building Logfile
using System.Text;
//
using System.Collections;
using UnityEngine;
using UWE;

namespace ModInstalLogger.Patches
{
    class LoggerLogic
    {
        public static IEnumerator ShowIngameMessage_async(string Message)
        {
            //It seems that the Filewriter Process need some time after saving. Otherwise the Game crashes.
            yield return new WaitForSecondsRealtime(2);
            yield return new WaitForSeconds(4);
            ErrorMessage.AddMessage(Message);
        }

        public static void ShowIngameMessage(string Message)
        {
            //QModServices.Main.AddCriticalMessage(""); //add ingame message that stays also a bit after loading
            //Console.WriteLine(""); //Just write to Log
            //ErrorMessage.AddMessage(""); //Shows Nothing ?
            //QModServices.Main.AddCriticalMessage(Message,15,"white");

            try
            {
                //We never use this but be catch a error here. Yeah its hacky.....
                string Indicator = Player.main.name;

                //We are in a Savegame

                //No clue how to display something
                CoroutineHost.StartCoroutine(ShowIngameMessage_async(Message));
                
            }
            catch
            {
                //We are in a Main Menu
                QModServices.Main.AddCriticalMessage(Message, 15, "white");
            }
        }

        public static void ModCompare(List<Moddata> Modlistexist, List<Moddata> Modlistnew, string Path_added, string Path_removed, string Reason = "none")
        {
            List<Moddata> firstNotSecond = new List<Moddata>();
            List<Moddata> secondNotFirst = new List<Moddata>();

            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Start creating list firstNotSecond");
            foreach (Moddata Modexist in Modlistexist)
            {
                bool notfound = true;
                foreach (Moddata Modnew in Modlistnew)
                {
                    //Compare Core informations to indicate if Mod is changed in any way.
                    //use Shortcut Compare to reduce workload
                    if (Modexist.ID == Modnew.ID && Modexist.Author == Modnew.Author && Modexist.Version == Modnew.Version)
                    {
                        notfound = false;
                        break;
                    }
                }
                if (notfound)
                {
                    firstNotSecond.Add(Modexist);
                }
            }

            MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, "Start creating list secondNotFirst");
            foreach (Moddata Modnew in Modlistnew)
            {
                bool notfound = true;
                foreach (Moddata Modexist in Modlistexist)
                {
                    //Compare Core informations to indicate if Mod is changed in any way.
                    //use Shortcut Compare to reduce workload
                    if (Modexist.ID == Modnew.ID && Modexist.Author == Modnew.Author && Modexist.Version == Modnew.Version)
                    {
                        notfound = false;
                        break;
                    }
                }
                if (notfound)
                {
                    secondNotFirst.Add(Modnew);
                }
            }

            MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, "Start checking list firstNotSecond");
            if (firstNotSecond.Count == 0)
            {
                MyLogger.Logger.Log(MyLogger.Logger.Level.Info, $"No Mod was removed from {Reason} compared to last time.");
                ShowIngameMessage($"No Mod was removed from {Reason} compared to last time.");
            }
            else
            {
                /*
                foreach (Moddata y in firstNotSecond)
                {
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, $"--- Mod was removed from {Reason} ---");
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, y.ID);
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, y.Author);
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, y.Displayname);
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, y.Enabled.ToString());
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, y.Version.ToString());
                }
                */

                if (ModInstalLogger.Config.ShowIngameNotificationonChange)
                {
                    ShowIngameMessage($"{firstNotSecond.Count} Mods were removed from {Reason}");
                }
            }

            MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, "Start checking list secondNotFirst");
            if (secondNotFirst.Count == 0)
            {
                MyLogger.Logger.Log(MyLogger.Logger.Level.Info, $"No Mod was added from {Reason} compared to last time.");
                ShowIngameMessage($"No Mod was added from {Reason} compared to last time.");
            }
            else
            {
                /*
                foreach (Moddata x in secondNotFirst)
                {
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, $"--- New Mod added from {Reason} ---");
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, x.ID);
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, x.Author);
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, x.Displayname);
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, x.Enabled.ToString());
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, x.Version.ToString());
                }
                */

                if (ModInstalLogger.Config.ShowIngameNotificationonChange)
                {
                    ShowIngameMessage($"{secondNotFirst.Count} Mods were added to {Reason}");
                }
            }

            //Set Format for JSON File so its readable without Transforming for example with Notepad++ Plugin
            MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, "Creating Formatting");
            Formatting myformat = new Formatting();
            myformat = Formatting.Indented;

            //Write List with removed Mods to Filesystem
            MyLogger.Logger.Log(MyLogger.Logger.Level.Debug, $"Start saving Files - {Reason}");
            string json_removed = JsonConvert.SerializeObject(firstNotSecond, myformat);
            try
            {
                File.WriteAllText(Path_removed, json_removed);
                MyLogger.Logger.Log(MyLogger.Logger.Level.Info, $"Removed Mod List for {Reason} was saved");
            }
            catch 
            {
                MyLogger.Logger.Log(MyLogger.Logger.Level.Error, "ErrorID:101 - Saving Compare List File failed");
            }
            

            //Read List with removed Mods to Filesystem
            string json_added = JsonConvert.SerializeObject(secondNotFirst, myformat);
            try
            {
                File.WriteAllText(Path_added, json_added);
                MyLogger.Logger.Log(MyLogger.Logger.Level.Info, $"Added Mod List for {Reason} was saved");
            }
            catch
            {
                MyLogger.Logger.Log(MyLogger.Logger.Level.Error, "ErrorID:102 - Saving Compare List File failed");
            }
        }

        public static List<Moddata> GetrunningMods()
        {
            //Get All runing Mods
            var mods = QModServices.Main.GetAllMods();
            //create Empty List for Mods
            List<Moddata> mymodlist = new List<Moddata>();
            foreach (var mod in mods)
            {
                try
                {
                    //only add the Mod if Active
                    if (mod.Enable)
                    {
                        Moddata tmp_mod = new Moddata();
                        tmp_mod.Author = mod.Author;
                        tmp_mod.Displayname = mod.DisplayName;
                        tmp_mod.Enabled = mod.Enable;
                        tmp_mod.ID = mod.Id;
                        tmp_mod.Version = mod.ParsedVersion;

                        mymodlist.Add(tmp_mod);
                    }
                }
                catch
                {
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Error, "ErrorID:104 - On creating Template for following Mod");
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Error, mod.Id);
                }

            }

            return mymodlist;
        }

        public static List<Moddata> GetdisabledMods()
        {
            //Get All runing Mods
            var mods = QModServices.Main.GetAllMods();
            //create Empty List for Mods
            List<Moddata> mymodlist = new List<Moddata>();
            foreach (var mod in mods)
            {
                try
                {
                    //only add the Mod if Active
                    if (!mod.Enable)
                    {
                        Moddata tmp_mod = new Moddata();
                        tmp_mod.Author = mod.Author;
                        tmp_mod.Displayname = mod.DisplayName;
                        tmp_mod.Enabled = mod.Enable;
                        tmp_mod.ID = mod.Id;
                        tmp_mod.Version = new Version("0.0.0.0");

                        mymodlist.Add(tmp_mod);
                    }
                }
                catch
                {
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Error, "ErrorID:114 - On creating Template for following Mod");
                    MyLogger.Logger.Log(MyLogger.Logger.Level.Error, mod.Id);
                }

            }

            return mymodlist;
        }

        public static void WriteReadableUserModList(string Savepath, List<Moddata> mymodlist, string reason)
        {
            //Phase 2.1 - Init 
            StringBuilder stringBuilder = new StringBuilder();
            string timeStamp = DateTime.Now.ToString("yyyy.MM.dd - HH:mm");
            stringBuilder.AppendLine("Mod Instal Logger - Modlist Export");
            stringBuilder.AppendLine($"This Export was created at {timeStamp}");
            stringBuilder.AppendLine($"- - - --- - - - --- --- - - - --- - - - --- --- - - - --- - - -");


            if (!business.itsok())
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

            List<Moddata> enabledmods = mymodlist;
            List<Moddata> disabledmods = LoggerLogic.GetdisabledMods();

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
                File.WriteAllText(Savepath, stringBuilder.ToString());
                MyLogger.Logger.Log(MyLogger.Logger.Level.Info, $"User Readable Modlist was saved for {reason}.");
            }
            catch
            {
                MyLogger.Logger.Log(MyLogger.Logger.Level.Error, $"ErrorID:202 - Saving User Readable Modlist File failed for {reason}");
            }
        }
    }
}
