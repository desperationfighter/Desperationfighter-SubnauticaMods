﻿using HarmonyLib;
//for CustomClass and User Configsetting
using ModInstalLogger.Management;
//for List
using System.Collections.Generic;
//for File Operations
using System.IO;
//for JSON Operation
using Oculus.Newtonsoft.Json;
//for getting Savegame infos from Game
using SMLHelper.V2.Utility;

namespace ModInstalLogger.Patches
{
    [HarmonyPatch(typeof(uGUI_OptionsPanel), nameof(uGUI_OptionsPanel.AddTabs))]
    internal static class uGui_OptionsPanel_Patch
    {
        internal static string ChangedModsTabName = "Changed Mods";
        internal static int ChangedModsTab;

        [HarmonyPostfix]
        internal static void Postfix(uGUI_OptionsPanel __instance)
        {
            ChangedModsTab = __instance.AddTab(ChangedModsTabName);

            if(!business.itsok())
            {
                __instance.AddHeading(ChangedModsTab, $"- - - --- - - - --- --- - - - --- - - - --- --- - - - --- - - -");
                __instance.AddHeading(ChangedModsTab, $"] - --- - - - --- <<< ||| \\\\\\ /// ||| >>> --- - - - --- - [");
                __instance.AddHeading(ChangedModsTab, $"- - - --- - - - --- --- - - - --- - - - --- --- - - - --- - - -");
                __instance.AddHeading(ChangedModsTab, $"Thank you for using my Mod and enjoying all the Other Mods.");
                __instance.AddHeading(ChangedModsTab, $"If you like the Game that much you already Mod it,");
                __instance.AddHeading(ChangedModsTab, $"may think about buying it. Pirating a Game is not a nice thing.");
                __instance.AddHeading(ChangedModsTab, $"- - - --- - - - --- --- - - - --- - - - --- --- - - - --- - - -");
                __instance.AddHeading(ChangedModsTab, $"] - --- - - - --- <<< ||| /// \\\\\\ ||| >>> --- - - - --- - [");
                __instance.AddHeading(ChangedModsTab, $"- - - --- - - - --- --- - - - --- - - - --- --- - - - --- - - -");
            }
         
            try
            {
                //We never use this but be catch a error here. Yeah its hacky.....
                string Indicator= Player.main.name;

                __instance.AddHeading(ChangedModsTab, $"> Current View: Savegame individual changes.");

                // --- --- --- Savegame - Removing
                __instance.AddHeading(ChangedModsTab, $"-- List of removed Mods compared to last time you played this Savegame ---");

                //Get Current Savegame Slot
                string CurrentSavegameDatadir = SaveUtils.GetCurrentSaveDataDir();
                string CurrentSavegameDatadir_remove = Player_Awake_Patch.GetPath_SavegameModListChange_Removed(CurrentSavegameDatadir);
                string CurrentSavegameDatadir_add = Player_Awake_Patch.GetPath_SavegameModListChange_Added(CurrentSavegameDatadir);

                if (File.Exists(CurrentSavegameDatadir_remove))
                {
                    List<Moddata> ExistingModList = JsonConvert.DeserializeObject<List<Moddata>>(File.ReadAllText(CurrentSavegameDatadir_remove));
                    if (ExistingModList.Count == 0)
                    {
                        __instance.AddHeading(ChangedModsTab, $"No Mods were removed compared to last Time");
                    }
                    else
                    {
                        foreach (Moddata moddata in ExistingModList)
                        {
                            __instance.AddHeading(ChangedModsTab, $"{moddata.Displayname} from {moddata.Author}");
                        }
                    }
                }
                else
                {
                    __instance.AddHeading(ChangedModsTab, $"There is no previouis Mod list file to indicate changes.");
                }

                // --- --- --- Savegame - Adding
                __instance.AddHeading(ChangedModsTab, $"-- List of added Mods compared to last time you played this Savegame ---");
                if (File.Exists(CurrentSavegameDatadir_add))
                {
                    List<Moddata> ExistingModList = JsonConvert.DeserializeObject<List<Moddata>>(File.ReadAllText(CurrentSavegameDatadir_add));
                    if (ExistingModList.Count == 0)
                    {
                        __instance.AddHeading(ChangedModsTab, $"No Mods were added compared to last Time");
                    }
                    else
                    {
                        foreach (Moddata moddata in ExistingModList)
                        {
                            __instance.AddHeading(ChangedModsTab, $"{moddata.Displayname} from {moddata.Author}");
                        }
                    }
                }
                else
                {
                    __instance.AddHeading(ChangedModsTab, $"There is no previouis Mod list file to indicate changes.");
                }
            }
            catch
            {
                __instance.AddHeading(ChangedModsTab, $"> Current View: Gamewide Mod changes.");

                // --- --- --- Game Wide - Removing
                __instance.AddHeading(ChangedModsTab, $"-- List of removed Mods compared to last Game start ---");
                if (File.Exists(Gameboot.GetPath_ModListChange_Removed()))
                {
                    List<Moddata> ExistingModList = JsonConvert.DeserializeObject<List<Moddata>>(File.ReadAllText(Gameboot.GetPath_ModListChange_Removed()));
                    if (ExistingModList.Count == 0)
                    {
                        __instance.AddHeading(ChangedModsTab, $"No Mods were removed compared to last Time");
                    }
                    else
                    {
                        foreach (Moddata moddata in ExistingModList)
                        {
                            __instance.AddHeading(ChangedModsTab, $"{moddata.Displayname} from {moddata.Author}");
                        }
                    }
                }
                else
                {
                    __instance.AddHeading(ChangedModsTab, $"There is no previouis Mod list file to indicate changes.");
                }

                // --- --- --- Game Wide - Adding
                __instance.AddHeading(ChangedModsTab, $"-- List of added Mods compared to last Game start---");
                if (File.Exists(Gameboot.GetPath_ModListChange_Added()))
                {
                    List<Moddata> ExistingModList = JsonConvert.DeserializeObject<List<Moddata>>(File.ReadAllText(Gameboot.GetPath_ModListChange_Added()));
                    if (ExistingModList.Count == 0)
                    {
                        __instance.AddHeading(ChangedModsTab, $"No Mods were added compared to last Time");
                    }
                    else
                    {
                        foreach (Moddata moddata in ExistingModList)
                        {
                            __instance.AddHeading(ChangedModsTab, $"{moddata.Displayname} from {moddata.Author}");
                        }
                    }
                }
                else
                {
                    __instance.AddHeading(ChangedModsTab, $"There is no previouis Mod list file to indicate changes.");
                }
            }
        }
    }
}