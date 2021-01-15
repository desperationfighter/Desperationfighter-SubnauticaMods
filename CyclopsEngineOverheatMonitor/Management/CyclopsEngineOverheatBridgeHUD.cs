using System.IO;
using System.Reflection;
using MoreCyclopsUpgrades.API;
using MoreCyclopsUpgrades.API.StatusIcons;
using SMLHelper.V2.Utility;
using CyclopsEngineOverheatMonitor.Patches;
using CyclopsEngineOverheatMonitor.Items;
using UnityEngine;

namespace CyclopsEngineOverheatMonitor.Management
{
    internal class CyclopsEngineOverheatBridgeHUD : CyclopsStatusIcon
    {
        private CylopsEngineOverheatModule cyEOM = new CylopsEngineOverheatModule();

        //public CyclopsEngineOverheatBridgeHUD(SubRoot cyclops) : base(cyclops)
        public CyclopsEngineOverheatBridgeHUD(SubRoot cyclops) : base(cyclops)
        {

        }

        public override string StatusText()
        {
            return Subfire_Patch.EngineTemperatur.ToString() ;
        }

        public override Color StatusTextColor()
        {
            Color color = new Color();
            if(Subfire_Patch.EngineTemperatur > 70)
            { color = Color.red; }
            else if(Subfire_Patch.EngineTemperatur < 20)
            { color = Color.green; }
            else
            { color = Color.yellow; }
            return color;
        }

        public override Atlas.Sprite StatusSprite()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"CylopsEngineOverheatModule.png");
            return ImageUtils.LoadSpriteFromFile(path);
        }

        public override bool ShowStatusIcon => MCUServices.CrossMod.HasUpgradeInstalled(base.Cyclops, cyEOM.TechType);
    }
}
