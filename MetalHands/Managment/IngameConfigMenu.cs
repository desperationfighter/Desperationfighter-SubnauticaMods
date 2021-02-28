﻿using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace MetalHands.Managment
{
    [Menu("Metal Hands Settings")]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Toggle("Enable This Mod", Tooltip = "This Enable/Diable the mod functionality", Order = 1)]
        public bool Config_ModEnable = true;

        [Toggle("Hardcoremode (require Restart)", Tooltip = "This Change the requirement for the Blueprint. Require Restart.", Order = 2)]
        public bool Config_Hardcore = false;
        
        [Toggle("Fast break without Glove",Tooltip = "Enable = Player can break Ressource fast without Glove")]
        public bool Config_fastbreak = false;

        [Toggle("Fast Collect without Glove",Tooltip = "Enable = Add spawned Ressouce from Ressouce brake directly to ")]
        public bool Config_fastcollect = false;
    }
}