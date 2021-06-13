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
//internal
using Snowfoxcloak.Managment;
using Snowfoxcloak.Items;

namespace Snowfoxcloak
{
    [QModCore]
    public static class Snowfoxcloak
    {
        internal static IngameConfigMenu Config { get; private set; }
        internal static TechType SnowfoxCloakModuleTechType { get; private set; }

        [QModPatch]
        public static void Snowfoxcloak_InitializationMethod()
        {
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();

            var SnowfoxCloakModulBlueprint = new SnowfoxCloakModule();
            SnowfoxCloakModulBlueprint.Patch();
            SnowfoxCloakModuleTechType = SnowfoxCloakModulBlueprint.TechType;

            Logger.Log(Logger.Level.Debug, "Snowfoxcloak Initialization");
            Harmony harmony = new Harmony("Snowfoxcloak");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "Snowfoxcloak Patched");

            //QModServices.Main.AddCriticalMessage("Warning the MetalHands Mod is in BETA Status !");
        }
    }
}
