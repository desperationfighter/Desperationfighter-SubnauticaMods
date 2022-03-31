using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace ModInstalLogger.Management
{
    [Menu("Mod Instal Logger Settings", SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Toggle("Show Ingame Notification", Tooltip = "This Enable/Diable the Ingame Notification when Mods are changes. When disabled the changes are only written to Logfile", Order = 1)]
        public bool ShowIngameNotificationonChange = true;

        [Toggle("[DEV] Debug Deep Logging", Tooltip = "This Enable/Diable Developer Deep Debug Logging", Order = 2)]
        public bool Debug_DeepLogging = false;
    }
}