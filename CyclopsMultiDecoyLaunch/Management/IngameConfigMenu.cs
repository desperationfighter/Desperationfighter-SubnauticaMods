using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace CyclopsMultiDecoyLaunch.Management
{
    [Menu("Cyclops Multi Decoy Launch", SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Slider("Additional Decoys to deploy", 0, 4, Step = 1, DefaultValue = 2, Tooltip = "[Default=1 / Vanilla=0] Settings for how much Additional Decoys will be launched", Order = 1)]
        public int AdditionalDecoystodeploy = 2;

        /*
        [Toggle("Enable Increased Decoy Storage", Tooltip = "This Enable/Diable the Over functionality for Inter Mod Compertibility. In case a other Mod changes this Settings and there is no Fix for it. You can just disable it here manually.", Order = 2)]
        public bool EnableOverrideMaxDecoyStorage = true;

        [Slider("Override Maximum Decoy Storage", 5, 15, Step = 1, DefaultValue = 5, Tooltip = "[Default=9 / Vanilla=5] Settings for Maximum Decoy Storage Size. (You still need the Upgrade Modules.)", Order = 3)]
        public int OverrideMaxDecoyStorage = 9;
        */
    }
}
