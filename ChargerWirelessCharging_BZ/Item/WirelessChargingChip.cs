using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using System.IO;
using System.Reflection;
using UnityEngine;
using System.Collections;
using SMLHelper.V2.Utility;

namespace ChargerWirelessCharging_BZ.Item
{
    internal class WirelessChargingChip : Equipable
    {
        public static TechType TechTypeID { get; protected set; }
        public WirelessChargingChip() : base("WirelesschargingChip",
            "Wireless charging Chip",
            "This Chip managed Wireless charging for Loose Batteries in your Inventory. (Does not Stack for Loading Speed, build more Charger instead)")
        { }

        public override string DiscoverMessage => "Wireless Charging Chip discovered";
        public override EquipmentType EquipmentType { get; } = EquipmentType.Chip;
        public override Vector2int SizeInInventory => new Vector2int(1, 1);
        public override TechCategory CategoryForPDA => TechCategory.Equipment;
        public override TechGroup GroupForPDA => TechGroup.Personal;
        public override QuickSlotType QuickSlotType => QuickSlotType.Passive;
        public override CraftTree.Type FabricatorType => CraftTree.Type.Fabricator;
        public override TechType RequiredForUnlock => TechType.PowerCellCharger;
        public override float CraftingTime => 3f;
        public override string[] StepsToFabricatorTab => new string[] { "Personal", "Equipment" };
        //public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
        //public override string IconFileName => "WirelesschargingChip.png";

        protected override Sprite GetItemSprite()
        {
            return ImageUtils.LoadSpriteFromFile(Path.Combine(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets"), "WirelesschargingChip.png"));
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.MapRoomHUDChip);
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
                    new Ingredient(TechType.AdvancedWiringKit, 1),
                    new Ingredient(TechType.CopperWire,1),
                    new Ingredient(TechType.Titanium, 1),
                }
            };
        }
    }
}