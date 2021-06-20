using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using System.IO;
using System.Reflection;
using SMLHelper.V2.Utility;
using UnityEngine;
using System.Collections;

namespace Snowfoxcloak.Items
{
    internal class SnowfoxCloakModule : Equipable
    {
        public SnowfoxCloakModule() : base("SnowFoxCloakModule",
            "Snowfox Cloak Module",
            "Using Precuser Technology this Module increases the Iceworm Reduction effect massivly. With an IonCube the Module provides and way more effective Magnetic field that redouce nearly all noices")
        {}

        public override CraftTree.Type FabricatorType => CraftTree.Type.Workbench;
        public override EquipmentType EquipmentType => EquipmentType.HoverbikeModule;
        public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
        public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
        public override TechType RequiredForUnlock => TechType.HoverbikeIceWormReductionModule;
        public override float CraftingTime => 5f;
        public override Vector2int SizeInInventory => new Vector2int(1, 1);
        public override QuickSlotType QuickSlotType => QuickSlotType.Passive;
        public override string[] StepsToFabricatorTab => new string[] { "HoverbikeModule" };

        //protected override Sprite GetItemSprite()
        //{
            //return ImageUtils.LoadSpriteFromFile(Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets"), "MetalHandsClawModule.png"));
        //}

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.HoverbikeIceWormReductionModule);
            yield return task;
            GameObject prefab = task.GetResult();
            GameObject obj = GameObject.Instantiate(prefab);
            prefab.SetActive(false);

            gameObject.Set(obj);
        }

        protected override RecipeData GetBlueprintRecipe()
        {
            return new RecipeData()
            {
                craftAmount = 1,
                Ingredients =
                {
                    new Ingredient(TechType.PrecursorIonCrystal, 1),
                    new Ingredient(TechType.CopperWire, 2),
                    new Ingredient(TechType.Magnetite, 4),
                    new Ingredient(TechType.ComputerChip, 1),
                    new Ingredient(TechType.AdvancedWiringKit, 1),
                    new Ingredient(TechType.HoverbikeIceWormReductionModule,1)
                }
            };
        }
    }
}
