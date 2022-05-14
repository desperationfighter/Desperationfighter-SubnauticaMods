using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace ModInstalLogger_BZ.Management
{
    [Menu("Mod Instal Logger Settings", SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Toggle("Show Ingame Notification", Tooltip = "This Enable/Diable the Ingame Notification when Mods are changes. When disabled the changes are only written to Logfile", Order = 1)]
        public bool ShowIngameNotificationonChange = true;

        [Toggle("Export Readable/Sharable Modlist", Tooltip = "This Enable/Diable the automaticly Export of your Mod List to a Readable and shareable File in the Game Folder.", Order = 2)]
        public bool WriteUserreadableList = true;

        [Toggle("Include Deactivated Mods in Export", Tooltip = "This Enable/Diable writing down deactivated Mods to the Mod List Export (For sure take only effect when Export is enabled.", Order = 3)]
        public bool WriteUserreadableList_includedisabled = false;

        //[Toggle("[DEV] Debug Deep Logging", Tooltip = "This Enable/Diable Developer Deep Debug Logging", Order = 2)]
        //public bool Debug_DeepLogging = false;
    }
}