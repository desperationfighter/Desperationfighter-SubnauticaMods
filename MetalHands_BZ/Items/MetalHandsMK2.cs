using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using System.IO;
using System.Reflection;
using SMLHelper.V2.Utility;
using UnityEngine;
using System.Collections;

namespace MetalHands.Items
{
    internal class MetalHandsMK2 : Equipable
    {
        public MetalHandsMK2() : base("MetalHandsMK2",
            "Metal Improved Grav Gloves",
            "This Gloves have a Metal Improved Cover and a additionally Gravsystem")
        { }

        public override EquipmentType EquipmentType { get; } = EquipmentType.Gloves;
        public override Vector2int SizeInInventory => new Vector2int(2, 2);
        public override TechCategory CategoryForPDA => TechCategory.Equipment;
        public override TechGroup GroupForPDA => TechGroup.Personal;
        public override QuickSlotType QuickSlotType => QuickSlotType.Passive;
        public override CraftTree.Type FabricatorType => CraftTree.Type.Workbench;
        public override float CraftingTime => 4f;
        public override TechType RequiredForUnlock => MetalHands_BZ.MetalHandsMK1TechType;
        //public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
        //public override string IconFileName => "MetalHandsMK2.png";
        //public override bool HasSprite => true;

        protected override Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile(Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets"), "MetalHandsMK2.png"));
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.ReinforcedDiveSuit);
            yield return task;
            GameObject prefab = task.GetResult();
            GameObject obj = GameObject.Instantiate(prefab);
            prefab.SetActive(false);

            gameObject.Set(obj);
        }

        protected override RecipeData GetBlueprintRecipe()
        {
            if(MetalHands_BZ.Config.Config_Hardcore == false)
            {
                return new RecipeData()
                {
                    craftAmount = 1,
                    Ingredients =
                    {
                        new Ingredient(MetalHands_BZ.MetalHandsMK1TechType, 1),
                        new Ingredient(TechType.AramidFibers, 1),
                        new Ingredient(TechType.CopperWire, 1),
                        new Ingredient(TechType.Magnetite, 2),
                        new Ingredient(TechType.ComputerChip, 1),
                        new Ingredient(TechType.SnowStalkerFur, 2)
                    }
                };
            }
            else
            {
                return new RecipeData()
                {
                    craftAmount = 1,
                    Ingredients =
                    {
                        new Ingredient(MetalHands_BZ.MetalHandsMK1TechType, 1),
                        new Ingredient(TechType.AramidFibers, 2),
                        new Ingredient(TechType.CopperWire, 2),
                        new Ingredient(TechType.Magnetite, 4),
                        new Ingredient(TechType.AdvancedWiringKit, 1),
                        new Ingredient(TechType.Silicone, 1),
                        new Ingredient(TechType.SnowStalkerFur, 1)
                    }
                };
            }

        }
    }
}
