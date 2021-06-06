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
using DAATQS_BZ.Managment;
using System.IO;
using System;

namespace DAATQS_BZ
{
    [QModCore]
    public static class DAATQS_BZ_harmony
    {
        internal static IngameConfigMenu Config { get; private set; }

        [QModPatch]
        public static void DAATQS_BZ_InitializationMethod()
        {
            Logger.Log(Logger.Level.Debug, "DAATQS_BZ Initialization");

            //new Problem. If player update this mod he would override his custom list by accident by me. So i need to check if the file already exist.
            string AllowlistPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "AllowList.json");
            if ( File.Exists(AllowlistPath) )
            {
                //Installation already exist, do nothing
            }
            else
            {
                //first install create AllowList.
                string defaultText = "{" + Environment.NewLine + @"  ""TechType"": [""builder"", ""Knife"", ""Seaglide"", ""LaserCutter"",""HeatBlade"", ""RepulsionCannon"", ""airbladder"", ""flashlight"", ""welder"", ""scanner"" ]" + Environment.NewLine + "}";
                File.WriteAllText(AllowlistPath, defaultText);
            }

            Harmony harmony = new Harmony("DAATQS_BZ");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            //Add the Ingame Config for User
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();

            Logger.Log(Logger.Level.Info, "DAATQS_BZ Patched");

            // QModServices.Main.AddCriticalMessage("Warning the DAATQS Mod is in BETA Status !");
        }
    }
}