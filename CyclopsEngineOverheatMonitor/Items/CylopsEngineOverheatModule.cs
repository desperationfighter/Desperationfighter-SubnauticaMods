using System.IO;
using System.Reflection;
using MoreCyclopsUpgrades.API;
using MoreCyclopsUpgrades.API.Upgrades;
using SMLHelper.V2.Crafting;

namespace CyclopsEngineOverheatMonitor.Items
{
    internal class CylopsEngineOverheatModule : CyclopsUpgrade
    {
        
         public CylopsEngineOverheatModule() : base("CyclopsOverheatMonitorModule",
            "Cyclops Overheat Module",
            "Displays Engine Heat State for better Monitoring.")
         {

         }

        public override CraftTree.Type FabricatorType { get; } = CraftTree.Type.CyclopsFabricator;
        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
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
                    new Ingredient(TechType.Polyaniline, 1),
                }
            };
        }
    }
}
