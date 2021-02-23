using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace MetalHands.Managment
{
    [Menu("Metal Hands Settings")]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Toggle("Enable This Mod", Tooltip = "This Enable/Diable the COMPLETE MOD", Order = 1,LabelLanguageId = "Config_ModEnable")]
        public bool Config_ModEnable = true;

        [Toggle("Hardcoremode (require Restart)", Tooltip = "This Change the requirement for the Blueprint. Require Restart.", Order = 2)]
        public bool Config_Hardcore = false;

        [Toggle("DEV1")]
        public bool Config_DEV1 = false;
    }
}
