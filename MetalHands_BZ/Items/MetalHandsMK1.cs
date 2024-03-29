﻿using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using System.IO;
using System.Reflection;
using SMLHelper.V2.Utility;
using UnityEngine;
using System.Collections;

namespace MetalHands.Items
{
    internal class MetalHandsMK1 : Equipable
    {
        public MetalHandsMK1() : base("MetalHandsMK1",
            "Metal Improved Gloves",
            "This Gloves have a Metal Improved Cover and allow Working with Hard Matrials without hurting the Person who wear it. Warning this Personal Safty Equiment is for Passive use avoiding severe injury . Do not use it as Tool")
        {}

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
        public override TechType RequiredForUnlock => TechType.AcidOld;
        public override float CraftingTime => 3f;
        public override string[] StepsToFabricatorTab => new string[] { "Personal", "Equipment" };
        //public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"Assets");
        //public override string IconFileName => "MetalHandsMK1.png";
        //public override bool HasSprite => true;

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.ColdSuitGloves);
            yield return task;
            GameObject prefab = task.GetResult();
            GameObject obj = GameObject.Instantiate(prefab);
            prefab.SetActive(false);

            gameObject.Set(obj);
        }

        protected override Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile(Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets"), "MetalHandsMK1.png"));
        }

        protected override RecipeData GetBlueprintRecipe()
        {
            if (MetalHands_BZ.Config.Config_Hardcore == false)
            {
                return new RecipeData()
                {
                    craftAmount = 1,
                    Ingredients =
                    {
                        new Ingredient(TechType.PlasteelIngot, 1),
                        new Ingredient(TechType.Diamond, 2),
                        new Ingredient(TechType.FiberMesh, 2),
                        new Ingredient(TechType.Silicone, 1)
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
                        new Ingredient(TechType.PlasteelIngot, 2),
                        new Ingredient(TechType.Nickel,4),
                        new Ingredient(TechType.AramidFibers, 2),
                        new Ingredient(TechType.Silicone, 2),
                        new Ingredient(TechType.SnowStalkerFur, 2)
                    }
                };
            }
        }
    }
}
