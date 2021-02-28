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
    public class MetalHands
    {
        internal static IngameConfigMenu Config { get; private set; }
        internal static TechType GloveBlueprintTechType { get; private set; }
        internal static TechType GloveMK2BlueprintTechType { get; private set; }
        internal static TechType GRAVHANDBlueprintTechType { get; private set; }

        [QModPatch]
        public static void MetalHands_InitializationMethod()
        {
            Logger.Log(Logger.Level.Debug, "MetalHands Initialization");

            Harmony harmony = new Harmony("MetalHands");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            //Add new Workbbench space
            //CraftTreeHandler.AddTabNode(CraftTree.Type.Workbench, "BodyMenu", "Suit Upgrades", SpriteManager.Get(TechType.Stillsuit));

            //Adding My Glove to the game
            var GloveBlueprint = new MetalHands_Blueprint();
            GloveBlueprint.Patch();
            var GloveMK2Blueprint = new MetalHandsMK2();
            GloveMK2Blueprint.Patch();
            var GRAVHANDBlueprint = new Prawn_GravHand();
            GRAVHANDBlueprint.Patch();

            GloveBlueprintTechType = GloveBlueprint.TechType;
            GloveMK2BlueprintTechType = GloveMK2Blueprint.TechType;
            GRAVHANDBlueprintTechType = GRAVHANDBlueprint.TechType;

            //Add the Ingame Config for User
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();

            Logger.Log(Logger.Level.Info, "MetalHands Patched");

            QModServices.Main.AddCriticalMessage("Warning the MetalHands Mod is in BETA Status !");
        }
    }
}
