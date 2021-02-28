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
            "Metal Grav Hand Gloves",
            "This Gloves have a Metal Improved Cover and a additionally Gravsystem")
        { }

        public override EquipmentType EquipmentType { get; } = EquipmentType.Gloves;
        public override Vector2int SizeInInventory => new Vector2int(2, 2);
        public override TechCategory CategoryForPDA => TechCategory.Equipment;
        public override TechGroup GroupForPDA => TechGroup.Personal;
        public override string DiscoverMessage => "Metal Hands dicovered";
        public override CraftTree.Type FabricatorType => CraftTree.Type.Workbench;
        public override TechType RequiredForUnlock => MetalHands.GloveBlueprintTechType;

        public override GameObject GetGameObject()
        {
            GameObject originGlove_prefab = CraftData.GetPrefabForTechType(TechType.ReinforcedGloves);
            GameObject gameobj = Object.Instantiate(originGlove_prefab);
            return gameobj;
        }

        protected override TechData GetBlueprintRecipe()
        {
            MetalHands.Config.Load();
            if(MetalHands.Config.Config_Hardcore == false)
            {
                return new TechData()
                {
                    craftAmount = 1,
                    Ingredients =
                    {
                        new Ingredient(MetalHands.GloveBlueprintTechType, 1),
                        new Ingredient(TechType.PlasteelIngot, 1),
                        new Ingredient(TechType.Diamond, 2),
                        new Ingredient(TechType.Nickel,2),
                        new Ingredient(TechType.AramidFibers, 1),
                        new Ingredient(TechType.Silicone, 1)
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
                        new Ingredient(TechType.PlasteelIngot, 2),
                        new Ingredient(TechType.Nickel,10),
                        new Ingredient(TechType.Kyanite,2),
                        new Ingredient(TechType.AramidFibers, 2),
                        new Ingredient(TechType.Silicone, 2)
                    }
                };
            }

        }
    }
}
