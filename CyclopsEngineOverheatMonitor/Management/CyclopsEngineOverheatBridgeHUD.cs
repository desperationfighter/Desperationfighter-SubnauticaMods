using MoreCyclopsUpgrades.API.StatusIcons;
using MoreCyclopsUpgrades.API;
using CyclopsEngineOverheatMonitor.Items;
using UnityEngine;

namespace CyclopsEngineOverheat.Management
{
    internal class CyclopsEngineOverheatBridgeHUD : CyclopsStatusIcon
    {
        private CylopsEngineOverheatModule cyEOM = new CylopsEngineOverheatModule();

        public bool cyclopsEngineOverheatBridgeHUD(SubRoot cyclops, CyclopsStatusIcon icon)
        {
            bool hasUpgrade = MCUServices.CrossMod.HasUpgradeInstalled(cyclops,cyEOM.TechType);
            return hasUpgrade;
        }

        

        public override string StatusText()
        {
            throw new System.NotImplementedException();
        }

        public override Color StatusTextColor()
        {
            throw new System.NotImplementedException();
        }

        public override Atlas.Sprite StatusSprite()
        {
            throw new System.NotImplementedException();
        }

        public override bool ShowStatusIcon => throw new System.NotImplementedException();
    }
}
