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
//WorkingSpace
using MetalHands.Managment;
using MetalHands.Items;

namespace MetalHands
{
    [QModCore]
    public class MetalHands_BZ
    {
        internal static IngameConfigMenu Config { get; private set; }
        internal static TechType GloveBlueprintTechType { get; private set; }
        internal static TechType GloveMK2BlueprintTechType { get; private set; }
        internal static TechType GRAVHANDBlueprintTechType { get; private set; }

        [QModPatch]
        public static void MetalHands_InitializationMethod()
        {
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();

            var GloveBlueprint = new MetalHands_Blueprint();
            GloveBlueprint.Patch();
            GloveBlueprintTechType = GloveBlueprint.TechType;
            var GloveMK2Blueprint = new MetalHandsMK2();
            GloveMK2Blueprint.Patch();
            GloveMK2BlueprintTechType = GloveMK2Blueprint.TechType;
            var GRAVHANDBlueprint = new Prawn_GravHand();
            GRAVHANDBlueprint.Patch();
            GRAVHANDBlueprintTechType = GRAVHANDBlueprint.TechType;

            Logger.Log(Logger.Level.Debug, "MetalHands_BZ Initialization");
            Harmony harmony = new Harmony("MetalHands_BZ");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "MetalHands_BZ Patched");

            //QModServices.Main.AddCriticalMessage("Warning the MetalHands Mod is in BETA Status !");
        }
    }
}
