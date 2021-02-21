using System.IO;
using System.Reflection;
using MoreCyclopsUpgrades.API;
using MoreCyclopsUpgrades.API.Upgrades;
using SMLHelper.V2.Crafting;
using CyclopsEngineOverheatMonitor.Management;
using MoreCyclopsUpgrades.API.StatusIcons;

namespace CyclopsEngineOverheatMonitor.Items
{
    internal class CylopsEngineOverheatModule : CyclopsUpgrade
    {
        public CylopsEngineOverheatModule() : base("CyclopsOverheatMonitorModule",
           "Cyclops Overheat Module",
           "This Module adds an Engine Temperatur Overview to the Cyclops Bridge HUD. Also it adds an improved Cooling function.This Module is not Stackable.")
        {
            OnFinishedPatching += () =>
            {
                MCUServices.Register.CyclopsUpgradeHandler((SubRoot cyclops) =>
                { return new UpgradeHandler(this.TechType, cyclops) { MaxCount = 1 }; });

                MCUServices.Register.PdaIconOverlay(this.TechType, (uGUI_ItemIcon icon, InventoryItem upgradeModule) =>
                { return new CyclopsEngineOverheatIconOverlay(icon, upgradeModule); });

                MCUServices.Register.CyclopsStatusIcon<CyclopsEngineOverheatBridgeHUD>((SubRoot cyclops) =>
                {return new CyclopsEngineOverheatBridgeHUD(cyclops); });
            };
        }

        //public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
        public override string AssetsFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public override string IconFileName => "CylopsEngineOverheatModule.png";

        public override CraftTree.Type FabricatorType { get; } = CraftTree.Type.CyclopsFabricator;

        public override string[] StepsToFabricatorTab { get; } = MCUServices.CrossMod.StepsToCyclopsModulesTabInCyclopsFabricator;

        protected override TechData GetBlueprintRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients =
                {
                    new Ingredient(TechType.ComputerChip, 2),
                    new Ingredient(TechType.WiringKit, 4),
                    new Ingredient(TechType.CopperWire, 8),
                    new Ingredient(TechType.Polyaniline, 1)
                }
            };
        }
    }
}
