using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace ChargerWirelessCharging_BZ.Managment
{
    [Menu("Charger Wireless Charging Settings", SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Toggle("Enable This Mod", Tooltip = "[Default=Enabled] This Enable/Diable the mod functionality on the fly without unloading the mod. Good if you may need to disable it just for a moment while ingame", Order = 1)]
        public bool Config_modEnable = true;

        [Toggle("Charge Loose Batteries without Chip", Tooltip = "[Default=Disabled] If you don't like to craft the Equimentpart first and want to charge your Loose Batteries directly", Order = 2)]
        public bool Config_chargeLooseBatteryWithoutChip = false;

        [Slider("Max Player Distance to Charger", 20f, 120f, Step = 5f, DefaultValue = 40f, Tooltip = "[Default=40] Configure the Maximum Player Distance to a Charger to still load Batteries.")]
        public float Config_maxPlayerDistanceToCharger = 40f;

        [Slider("Wireless Charge Speed in %", 50f , 200f , Step = 5f, DefaultValue = 85f, Tooltip = "[Default=85] You want faster Wireless Charging ? No Problem here you go.")]
        public float Config_WirelessChargeSpeed = 85f;
    }
}