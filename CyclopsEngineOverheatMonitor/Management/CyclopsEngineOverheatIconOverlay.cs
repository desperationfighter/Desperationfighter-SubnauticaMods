using MoreCyclopsUpgrades.API.PDA;
using CyclopsEngineOverheatMonitor.Patches;

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
            base.MiddleText.TextString = Subfire_Patch.EngineTemperatur.ToString();
            //base.UpperText.TextString = "DEV Text Upper";
        }
    }
}
