﻿using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace MetalHands.Managment
{
    [Menu("Metal Hands Settings", SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Toggle("Enable This Mod", Tooltip = "This Enable/Diable the mod functionality", Order = 1)]
        public bool Config_ModEnable = true;

        [Toggle("Hardcoremode (require Restart)", Tooltip = "This Change the requirement for the Blueprint. Require Restart.", Order = 2)]
        public bool Config_Hardcore = false;
        
        [Toggle("(Cheat) Fast break without Glove", Tooltip = "Enable = Player can break Ressource fast without Glove", Order = 3)]
        public bool Config_fastbreak = false;

        [Toggle("(Cheat) Fast Collect without Glove",Tooltip = "Enable = Add spawned Ressouce from Ressouce brake directly to ", Order = 4)]
        public bool Config_fastcollect = false;

        //[Toggle("(compatibility) Force Mod to run Postfix", Tooltip = "This Setting Force the Mod to run as a Postfix. This helps to be working together with other Mods that modifiy the Same parts of the Game. The Mod detects some Mods that are known to be problematic..", Order = 5)]
        //public bool Config_forcepostfix = false;

        //public bool Config_forceprefix_opverridepostfix = false;

        //[Toggle("(Easy Mode) Increase Spawn", Tooltip = "Enable = Increase the Spawn Chances a little bit to makle it easier, does ")]
        //public bool Config_MoreSpawn = false;

        //[Slider("Difficulty Level 1=Easy 3=Hardcore", 1, 3, Step = 1, DefaultValue = 2, Tooltip = "Dummy")]
        //public int Config_difficulty = 2;
    }
}
