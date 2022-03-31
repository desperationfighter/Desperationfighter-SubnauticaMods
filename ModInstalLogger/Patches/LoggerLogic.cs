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
        public static void ModCompare(List<Moddata> Modlistexist, List<Moddata> Modlistnew, string Path_added, string Path_removed, string Reason = "none")
        {
            //https://www.delftstack.com/howto/csharp/compare-two-lists-in-csharp/
            //List<Moddata> firstNotSecond = Modlistnew.Where(i => !Modlistexist.Contains(i)).ToList();
            //List<Moddata> secondNotFirst = Modlistexist.Where(i => !Modlistnew.Contains(i)).ToList();
            //seems not to work with Custom Classes... And well i don't mind creating a custom Compare Object so i take the simple way.

            List<Moddata> firstNotSecond = new List<Moddata>();
            List<Moddata> secondNotFirst = new List<Moddata>();

            foreach (Moddata Modexist in Modlistexist)
            {
                bool notfound = true;
                foreach (Moddata Modnew in Modlistnew)
                {
                    //Compare Core informations to indicate if Mod is changed in any way.
                    //use Shortcut Compare to reduce workload
                    if (Modexist.ID == Modnew.ID && Modexist.Author == Modnew.Author && Modexist.Version_Revision == Modnew.Version_Revision && Modexist.Version_Build == Modnew.Version_Build && Modexist.Version_Minor == Modnew.Version_Minor && Modexist.Version_Major == Modnew.Version_Major)
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

            foreach (Moddata Modnew in Modlistnew)
            {
                bool notfound = true;
                foreach (Moddata Modexist in Modlistexist)
                {
                    //Compare Core informations to indicate if Mod is changed in any way.
                    //use Shortcut Compare to reduce workload
                    if (Modexist.ID == Modnew.ID && Modexist.Author == Modnew.Author && Modexist.Version_Revision == Modnew.Version_Revision && Modexist.Version_Build == Modnew.Version_Build && Modexist.Version_Minor == Modnew.Version_Minor && Modexist.Version_Major == Modnew.Version_Major)
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

            foreach (Moddata y in firstNotSecond)
            {
                if(ModInstalLogger.Config.Debug_DeepLogging)
                {
                    Logger.Log(Logger.Level.Debug, $"--- Mod was removed from {Reason} -----------------------------------------");
                    Logger.Log(Logger.Level.Debug, y.ID);
                    Logger.Log(Logger.Level.Debug, y.Author);
                    Logger.Log(Logger.Level.Debug, y.Displayname);
                    Logger.Log(Logger.Level.Debug, y.Enabled);
                    Logger.Log(Logger.Level.Debug, y.Version_combined);
                    Logger.Log(Logger.Level.Debug, "--- Mod was removed ENDE -----------------------------------------");
                }
                else
                {
                    Logger.Log(Logger.Level.Debug, $"--- Mod was removed from {Reason} -----------------------------------------");
                    Logger.Log(Logger.Level.Debug, y.ID);
                    Logger.Log(Logger.Level.Debug, y.Version_combined);
                }

                if (ModInstalLogger.Config.ShowIngameNotificationonChange)
                {
                    //QModServices.Main.AddCriticalMessage($"The Mod {y.Displayname} in Version {y.Version_combined} was removed",15);
                    Console.WriteLine($"The Mod {y.Displayname} in Version {y.Version_combined} was removed from {Reason}");
                }
            }
            foreach (Moddata x in secondNotFirst)
            {
                if (ModInstalLogger.Config.Debug_DeepLogging)
                {
                    Logger.Log(Logger.Level.Debug, $"--- New Mod added from {Reason} -----------------------------------------");
                    Logger.Log(Logger.Level.Debug, x.ID);
                    Logger.Log(Logger.Level.Debug, x.Author);
                    Logger.Log(Logger.Level.Debug, x.Displayname);
                    Logger.Log(Logger.Level.Debug, x.Enabled);
                    Logger.Log(Logger.Level.Debug, x.Version_combined);
                    Logger.Log(Logger.Level.Debug, "--- New Mod added ENDE -----------------------------------------");
                }
                else
                {
                    Logger.Log(Logger.Level.Debug, $"--- New Mod added from {Reason} -----------------------------------------");
                    Logger.Log(Logger.Level.Debug, x.ID);
                }

                if (ModInstalLogger.Config.ShowIngameNotificationonChange)
                {
                    //QModServices.Main.AddCriticalMessage($"The Mod {x.Displayname} in Version {x.Version_combined} was added", 13, "Green");
                    Console.WriteLine($"The Mod {x.Displayname} in Version {x.Version_combined} was added to {Reason}");
                }
            }

            //Set Format for JSON File so its readable without Transforming for example with Notepad++ Plugin
            Formatting myformat = new Formatting();
            myformat = Formatting.Indented;

            //Write List with removed Mods to Filesystem
            string json_removed = JsonConvert.SerializeObject(firstNotSecond, myformat);
            File.WriteAllText(Path_removed, json_removed);

            //Read List with removed Mods to Filesystem
            string json_added = JsonConvert.SerializeObject(secondNotFirst, myformat);
            File.WriteAllText(Path_added, json_added);
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
                    //Reduce Logfile Size when not DeepLogging
                    if(ModInstalLogger.Config.Debug_DeepLogging)
                    {
                        Logger.Log(Logger.Level.Debug, "--- One Mod info Extended -----------------------------------------");
                        Logger.Log(Logger.Level.Debug, mod.Id);
                        Logger.Log(Logger.Level.Debug, mod.AssemblyName);
                        Logger.Log(Logger.Level.Debug, mod.Author);
                        Logger.Log(Logger.Level.Debug, mod.DisplayName);
                        Logger.Log(Logger.Level.Debug, (mod.Enable).ToString());
                        if (mod.Enable)
                        {
                            Logger.Log(Logger.Level.Debug, mod.ParsedVersion.Major.ToString()); //1
                            Logger.Log(Logger.Level.Debug, mod.ParsedVersion.Minor.ToString()); //2
                            Logger.Log(Logger.Level.Debug, mod.ParsedVersion.Build.ToString()); //3
                            Logger.Log(Logger.Level.Debug, mod.ParsedVersion.MajorRevision.ToString()); //1alt
                            Logger.Log(Logger.Level.Debug, mod.ParsedVersion.MinorRevision.ToString()); //4alt
                            Logger.Log(Logger.Level.Debug, mod.ParsedVersion.Revision.ToString()); //4
                        }
                        Logger.Log(Logger.Level.Debug, "--- One Mod info ENDE -----------------------------------------");
                    }
                    else
                    {
                        Logger.Log(Logger.Level.Debug, "--- One Mod info Short-----------------------------------------");
                        Logger.Log(Logger.Level.Debug, mod.Id);
                        Logger.Log(Logger.Level.Debug, (mod.Enable).ToString());
                    }

                    //only add the Mod if Active
                    if (mod.Enable)
                    {
                        Moddata tmp_mod = new Moddata();
                        tmp_mod.Author = mod.Author;
                        tmp_mod.Displayname = mod.DisplayName;
                        tmp_mod.Enabled = (mod.Enable).ToString();
                        tmp_mod.ID = mod.Id;
                        tmp_mod.Version_Build = mod.ParsedVersion.Build.ToString();
                        tmp_mod.Version_Major = mod.ParsedVersion.Major.ToString();
                        tmp_mod.Version_Minor = mod.ParsedVersion.Minor.ToString();
                        tmp_mod.Version_Revision = mod.ParsedVersion.Revision.ToString();

                        mymodlist.Add(tmp_mod);
                    }
                }
                catch
                {
                    Logger.Log(Logger.Level.Error, "ErrorID:300 - On creating Template for following Mod");
                    Logger.Log(Logger.Level.Error, mod.Id);
                }
            }

            return mymodlist;
        }
    }
}
