using MoreCyclopsUpgrades.API.StatusIcons;
using MoreCyclopsUpgrades.API;
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
            return "23";
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
