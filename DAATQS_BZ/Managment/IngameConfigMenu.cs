using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace DAATQS_BZ.Managment
{
    [Menu("DAATQS_BZ Settings")]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Toggle("Enable This Mod", Tooltip = "This Enable/Diable the COMPLETE MOD", Order = 1)]
        public bool Config_ModEnable = true;

        [Toggle("Allow Items from the Custom List", Tooltip = "You can create your own TechType list of items that you want to still Auto-add even when the mod is running", Order = 2)]
        public bool Config_AllowCustomList = true;

    }

}