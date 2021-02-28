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
            "Metal Hand Gloves",
            "This Gloves have a Metal Improved Cover and allow Working with Hard Matrials without hurting the Person who wear it. Warning this Personal Safty Equiment is for Passive use avoiding severe injury . Do not use it as Tool")
        {
            //No idea what to do here ???
        }

        //On Work
        /*
        //decided to add the base version to fabricator so Player get it easier and the 
        //public override CraftTree.Type FabricatorType => CraftTree.Type.Workbench;

        public override PDAEncyclopedia.EntryData EncyclopediaEntryData => base.EncyclopediaEntryData;
        public override TechType RequiredForUnlock => TechType.ReinforcedDiveSuitBlueprint;
        public override string AssetsFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public override string IconFileName => ".png";
        */

        public override EquipmentType EquipmentType { get; } = EquipmentType.Gloves;
        public override Vector2int SizeInInventory => new Vector2int(2, 2);
        public override TechCategory CategoryForPDA => TechCategory.Equipment;
        public override TechGroup GroupForPDA => TechGroup.Personal;
        public override string DiscoverMessage => "Improved Ressource collection Tools dicovered";

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
                        //new Ingredient(TechType.ReinforcedGloves, 1),
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
                        //new Ingredient(TechType.ReinforcedGloves, 1),
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
