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
    internal class MetalHands_Blueprint : Equipable
    {
        public MetalHands_Blueprint() : base("MetalHands",
            "Metal Improved Gloves",
            "This Gloves have a Metal Improved Cover and allow Working with Hard Matrials without hurting the Person who wear it. Warning this Personal Safty Equiment is for Passive use avoiding severe injury . Do not use it as Tool")
        {
            //No idea what to do here ???
        }

        //On Work
        /*
        public override PDAEncyclopedia.EntryData EncyclopediaEntryData => base.EncyclopediaEntryData;

        */

        public override string DiscoverMessage => "Improved Metal Gloves dicovered";
        public override EquipmentType EquipmentType { get; } = EquipmentType.Gloves;
        public override Vector2int SizeInInventory => new Vector2int(2, 2);
        public override TechCategory CategoryForPDA => TechCategory.Equipment;
        public override TechGroup GroupForPDA => TechGroup.Personal;
        public override QuickSlotType QuickSlotType => QuickSlotType.Passive;
        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        public override float CraftingTime => 2f;
        public override string[] StepsToFabricatorTab => new string[] { "Personal", "Equipment" };
        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"Assets");
        public override string IconFileName => "reinforcedgloves.png";

        /*
        public override GameObject GetGameObject()
        {
            GameObject originGlove_prefab = CraftData.GetPrefabForTechType(TechType.ReinforcedGloves);
            GameObject gameobj = Object.Instantiate(originGlove_prefab);
            return gameobj;
        }
        */

        protected override RecipeData GetBlueprintRecipe()
        {
            if (MetalHands_BZ.Config.Config_Hardcore == false)
            {
                return new RecipeData()
                {
                    craftAmount = 1,
                    Ingredients =
                    {
                        new Ingredient(MetalHands_BZ.GloveBlueprintTechType, 1),
                        new Ingredient(TechType.AramidFibers, 1),
                        new Ingredient(TechType.CopperWire, 1),
                        new Ingredient(TechType.Magnetite, 2),
                        new Ingredient(TechType.ComputerChip, 1),
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
                        new Ingredient(MetalHands_BZ.GloveBlueprintTechType, 1),
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
