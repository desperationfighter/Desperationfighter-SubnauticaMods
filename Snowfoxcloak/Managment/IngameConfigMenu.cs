using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace Snowfoxcloak.Managment
{
    [Menu("Snowfox Cloak", SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        //[Toggle("Enable This Mod", Tooltip = "This Enable/Diable the mod functionality", Order = 1)]
        //public bool Config_ModEnable = true;

        [Toggle("Full Cloak", Tooltip = "This Settings turn the Cloak Module to 100% so that the Snowfox is invisible to the Iceworm. WARNING: That does not mean the Iceworm is not triggered ! It only says the Snowfox does not make any Voice that increase the trigger for the Iceworm ;-)", Order = 2)]
        public bool Config_Fullcloak = false;
    }
}
