using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using MetalHands.Managment;

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SMLHelper.V2.Utility;
using UnityEngine;
using SMLHelper.V2.Handlers;

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
        public override float CraftingTime => 3f;
        public override TechType RequiredForUnlock => MetalHands.GloveBlueprintTechType;
        //public override string[] StepsToFabricatorTab => new string[] { "Personal"};
        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
        public override string IconFileName => "MetalHandsMK2.png";

        public override GameObject GetGameObject()
        {
            GameObject originGlove_prefab = CraftData.GetPrefabForTechType(TechType.ReinforcedGloves);
            GameObject gameobj = Object.Instantiate(originGlove_prefab);
            return gameobj;
        }

        protected override TechData GetBlueprintRecipe()
        {
            if(MetalHands.Config.Config_Hardcore == false)
            {
                return new TechData()
                {
                    craftAmount = 1,
                    Ingredients =
                    {
                        new Ingredient(MetalHands.GloveBlueprintTechType, 1),
                        new Ingredient(TechType.AramidFibers, 1),
                        new Ingredient(TechType.CopperWire, 1),
                        new Ingredient(TechType.Magnetite, 2),
                        new Ingredient(TechType.ComputerChip, 1),
                    }
                };
            }
            else
            {
                return new TechData()
                {
                    craftAmount = 1,
                    Ingredients =
                    {
                        new Ingredient(MetalHands.GloveBlueprintTechType, 1),
                        new Ingredient(TechType.AramidFibers, 2),
                        new Ingredient(TechType.CopperWire, 2),
                        new Ingredient(TechType.Magnetite, 4),
                        new Ingredient(TechType.AdvancedWiringKit, 1)
                    }
                };
            }

        }
    }
}
