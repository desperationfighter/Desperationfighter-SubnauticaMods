using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace CargerWirelessCharging.Item
{
    internal class WirelessChargingChip : Equipable
    {
        public static TechType TechTypeID { get; protected set; }
        public WirelessChargingChip() : base("WirelessChargingChip",
            "Wireless Charging Chip",
            "This Chip managed Wireless charging for Loose Batteries in your Inventory.")
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
        public override string AssetsFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
        //public override string IconFileName => ".png";
        public override GameObject GetGameObject()
        {
            GameObject originGlove_prefab = CraftData.GetPrefabForTechType(TechType.MapRoomHUDChip);
            GameObject gameobj = Object.Instantiate(originGlove_prefab);
            return gameobj;
        }
        protected override TechData GetBlueprintRecipe()
        {
            return new TechData()
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