//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//to show a test
using QModManager.API;
//for Logging
using QModManager.Utility;
//For IngameConfig
using SMLHelper.V2.Handlers;
//working space
using DAATQS.Managment;
using System.IO;
using System;

namespace DAATQS
{
    [QModCore]
    public static class DAATQS_harmony
    {
        internal static IngameConfigMenu Config { get; private set; }

        [QModPatch]
        public static void DAATQS_InitializationMethod()
        {
            Logger.Log(Logger.Level.Debug, "DAATQS Initialization");

            //new Problem. If player update this mod he would override his custom list by accident by me. So i need to check if the file already exist.
            string AllowlistPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AllowList.json");
            if (File.Exists(AllowlistPath))
            {
                //Installation already exist, do nothing
            }
            else
            {
                //first install create AllowList.
                string defaultText = "{" + Environment.NewLine + @"  ""TechType"": [ ""TEST"", ""builder"", ""Knife"", ""Seaglide"", ""LaserCutter"",""HeatBlade"", ""RepulsionCannon"", ""airbladder"", ""flashlight"", ""welder"", ""scanner"" ]" + Environment.NewLine + "}";
                File.WriteAllText(AllowlistPath, defaultText);
            }

            Harmony harmony = new Harmony("DAATQS");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            //Add the Ingame Config for User
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();

            Logger.Log(Logger.Level.Info, "DAATQS Patched");

            // QModServices.Main.AddCriticalMessage("Warning the DAATQS Mod is in BETA Status !");
        }
    }
}
