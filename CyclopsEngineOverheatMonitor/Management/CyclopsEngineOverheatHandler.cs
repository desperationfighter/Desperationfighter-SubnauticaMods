using MoreCyclopsUpgrades.API;
using MoreCyclopsUpgrades.API.General;
using MoreCyclopsUpgrades.API.Upgrades;
using UnityEngine;

using CyclopsEngineOverheatMonitor.Items;

namespace CyclopsEngineOverheatMonitor.Management
{
    class CyclopsEngineOverheatHandler : UpgradeHandler
    {
        //private readonly CylopsEngineOverheatModule cylopsEngineOverheatMonitorModule;
        private CyclopsMotorMode motorMode;
        private CyclopsMotorMode MotorMode => motorMode ?? (motorMode = base.Cyclops.GetComponentInChildren<CyclopsMotorMode>());
        
        public CyclopsEngineOverheatHandler(CylopsEngineOverheatModule cylopsEngineOverheatModule, SubRoot cyclops) : base(cylopsEngineOverheatModule.TechType, cyclops)
        {


        }
    }
}
