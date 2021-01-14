    using MoreCyclopsUpgrades.API;
    using MoreCyclopsUpgrades.API.PDA;
    using UnityEngine;

namespace CyclopsEngineOverheatMonitor.Management
{
    internal class CyclopsEngineOverheatIconOverlay : IconOverlay
    {        
        public CyclopsEngineOverheatIconOverlay(uGUI_ItemIcon icon, InventoryItem upgradeModule) : base(icon, upgradeModule)
        {

        }

        public override void UpdateText()
        {
            //base.LowerText.TextString = "DEV Text Lower";
            base.MiddleText.TextString = "DEV Text Middle";
            //base.UpperText.TextString = "DEV Text Upper";
        }
    }
}
