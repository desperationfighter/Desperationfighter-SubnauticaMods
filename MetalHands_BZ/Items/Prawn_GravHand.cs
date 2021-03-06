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
    internal class Prawn_GravHand : Equipable//Craftable
    {
        public Prawn_GravHand() : base("GravHand",
            "Grav Hand Plugin",
            "This Plugin intregates a Gravitations function to the PRAWN Hands and allow a shortrange "
            )
        { }
        public override CraftTree.Type FabricatorType => CraftTree.Type.SeamothUpgrades;
        public override EquipmentType EquipmentType => EquipmentType.ExosuitModule;
        public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
        public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
        public override TechType RequiredForUnlock => MetalHands_BZ.GloveBlueprintTechType;
        public override float CraftingTime => 3f;
        public override Vector2int SizeInInventory => new Vector2int(1, 1);
        public override QuickSlotType QuickSlotType => QuickSlotType.Passive;
        public override string[] StepsToFabricatorTab => new string[] { "ExosuitModules" };
        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
        public override string IconFileName => "reinforcedgloves.png";

        protected override RecipeData GetBlueprintRecipe()
        {
            if (MetalHands_BZ.Config.Config_Hardcore == false)
            {
                return new RecipeData()
                {
                    craftAmount = 1,
                    Ingredients =
                    {
                        new Ingredient(TechType.Titanium, 2),
                        new Ingredient(TechType.Diamond, 1),
                        new Ingredient(TechType.CopperWire, 1),
                        new Ingredient(TechType.Magnetite, 2),
                        new Ingredient(TechType.ComputerChip, 1),
                        new Ingredient(TechType.WiringKit, 1)
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
                        new Ingredient(TechType.TitaniumIngot, 1),
                        new Ingredient(TechType.Diamond, 2),
                        new Ingredient(TechType.CopperWire, 4),
                        new Ingredient(TechType.Magnetite, 4),
                        new Ingredient(TechType.AdvancedWiringKit, 2)
                    }
                };
            }

        }
    }
}
