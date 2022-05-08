//for Console Output
using System;
//for File Operations
using System.IO;
//for calling isntalled Mods
using QModManager.API;
//for Logging
using QModManager.Utility;
//for List
using System.Collections.Generic;
//for CustomClass and User Configsetting
using ModInstalLogger.Management;
//for JSON Formatting
using Oculus.Newtonsoft.Json;


namespace ModInstalLogger.Patches
{
    class LoggerLogic
    {
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
                ErrorMessage.AddMessage(Message);
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

            Logger.Log(Logger.Level.Debug, "Start creating list firstNotSecond");
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

            Logger.Log(Logger.Level.Debug, "Start creating list secondNotFirst");
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

            Logger.Log(Logger.Level.Debug, "Start checking list firstNotSecond");
            if (firstNotSecond.Count == 0)
            {
                Logger.Log(Logger.Level.Info, $"No Mod was removed from {Reason} compared to last time.");
                ShowIngameMessage($"No Mod was removed from {Reason} compared to last time.");
            }
            else
            {
                /*
                foreach (Moddata y in firstNotSecond)
                {
                    Logger.Log(Logger.Level.Debug, $"--- Mod was removed from {Reason} ---");
                    Logger.Log(Logger.Level.Debug, y.ID);
                    Logger.Log(Logger.Level.Debug, y.Author);
                    Logger.Log(Logger.Level.Debug, y.Displayname);
                    Logger.Log(Logger.Level.Debug, y.Enabled.ToString());
                    Logger.Log(Logger.Level.Debug, y.Version.ToString());
                }
                */

                if (ModInstalLogger.Config.ShowIngameNotificationonChange)
                {
                    ShowIngameMessage($"{firstNotSecond.Count} Mods were removed from {Reason}");
                }
            }

            Logger.Log(Logger.Level.Debug, "Start checking list secondNotFirst");
            if (secondNotFirst.Count == 0)
            {
                Logger.Log(Logger.Level.Info, $"No Mod was added from {Reason} compared to last time.");
                ShowIngameMessage($"No Mod was added from {Reason} compared to last time.");
            }
            else
            {
                /*
                foreach (Moddata x in secondNotFirst)
                {
                    Logger.Log(Logger.Level.Debug, $"--- New Mod added from {Reason} ---");
                    Logger.Log(Logger.Level.Debug, x.ID);
                    Logger.Log(Logger.Level.Debug, x.Author);
                    Logger.Log(Logger.Level.Debug, x.Displayname);
                    Logger.Log(Logger.Level.Debug, x.Enabled.ToString());
                    Logger.Log(Logger.Level.Debug, x.Version.ToString());
                }
                */

                if (ModInstalLogger.Config.ShowIngameNotificationonChange)
                {
                    ShowIngameMessage($"{secondNotFirst} Mods were added to {Reason}");
                }
            }

            //Set Format for JSON File so its readable without Transforming for example with Notepad++ Plugin
            Logger.Log(Logger.Level.Debug, "Creating Formatting");
            Formatting myformat = new Formatting();
            myformat = Formatting.Indented;

            //Write List with removed Mods to Filesystem
            Logger.Log(Logger.Level.Debug, $"Start saving Files - {Reason}");
            string json_removed = JsonConvert.SerializeObject(firstNotSecond, myformat);
            try
            {
                File.WriteAllText(Path_removed, json_removed);
                Logger.Log(Logger.Level.Info, $"Removed Mod List for {Reason} was saved");
            }
            catch 
            {
                Logger.Log(Logger.Level.Error, "ErrorID:101 - Saving Compare List File failed");
            }
            

            //Read List with removed Mods to Filesystem
            string json_added = JsonConvert.SerializeObject(secondNotFirst, myformat);
            try
            {
                File.WriteAllText(Path_added, json_added);
                Logger.Log(Logger.Level.Info, $"Added Mod List for {Reason} was saved");
            }
            catch
            {
                Logger.Log(Logger.Level.Error, "ErrorID:102 - Saving Compare List File failed");
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
                    Logger.Log(Logger.Level.Error, "ErrorID:104 - On creating Template for following Mod");
                    Logger.Log(Logger.Level.Error, mod.Id);
                }

            }

            return mymodlist;
        }
    }
}
