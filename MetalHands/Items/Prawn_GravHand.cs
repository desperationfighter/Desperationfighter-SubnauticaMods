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
    internal class Prawn_GravHand : Craftable
    {
        private static IngameConfigMenu ICM = new IngameConfigMenu();

        public Prawn_GravHand() : base("GravHand",
            "Grav Hand Plugin",
            "This Plugin intregates a Gravitations function to the PRAWN Hands and allow a shortrange "
            )
        { }

        public override TechCategory CategoryForPDA => TechCategory.VehicleUpgrades;
        public override CraftTree.Type FabricatorType => CraftTree.Type.Workbench;
        public override TechGroup GroupForPDA => TechGroup.VehicleUpgrades;
        public override TechType RequiredForUnlock => MetalHands.GloveBlueprintTechType;
        public override float CraftingTime => 2.5f;
        public override Vector2int SizeInInventory => new Vector2int(1, 1);

        protected override TechData GetBlueprintRecipe()
        {
            ICM.Load();
            if (ICM.Config_Hardcore == false)
            {
                return new TechData()
                {
                    craftAmount = 1,
                    Ingredients =
                    {
                        new Ingredient(TechType.Titanium, 1),
                        new Ingredient(TechType.CopperWire, 2),
                        new Ingredient(TechType.Magnetite, 2),
                        new Ingredient(TechType.ComputerChip, 1),
                        new Ingredient(TechType.WiringKit, 1)
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
                        new Ingredient(TechType.Titanium, 2),
                        new Ingredient(TechType.CopperWire, 4),
                        new Ingredient(TechType.Magnetite, 4),
                        new Ingredient(TechType.ComputerChip, 1),
                        new Ingredient(TechType.AdvancedWiringKit, 1)
                    }
                };
            }

        }
    }
}
