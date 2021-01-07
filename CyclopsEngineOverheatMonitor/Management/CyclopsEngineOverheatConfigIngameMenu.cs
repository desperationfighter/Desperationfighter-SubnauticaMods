using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace CyclopsEngineOverheatMonitor.Management
{
    [Menu("Cyclops Engine Overheat")]
    public class CyclopsEngineOverheatConfigIngameMenu : ConfigFile
    {
        public CyclopsEngineOverheatConfigIngameMenu() : base("config") { }

        [Toggle("[OVERRIDE]Disable Complete Heat Meachanic",Tooltip = "This option Disable the Complete Mod !")]
        public bool CyclopsHeat_general_disable = false;

        [Toggle("Disable First Overheat Warning")]
        public bool CyclopsHeat_prewarning_disable = false;

        [Toggle("Disable Second Overheat Warning")]
        public bool CyclopsHeat_mainwarning_disable = false;

        [Toggle("Disable Fire on Overheat")]
        public bool CyclopsHeat_FireonOverheat_disable = false;

        [Toggle("Double general Cooling Recover",Tooltip = "This options doubles the cooling effect. It applies for the Original Mechanic and the new and also and the low Temp Cooling Effect")]
        public bool CyclopsHeat_coolingrecover_double = false;

        [Toggle("Make the Meltdown Linear instead Random")]
        public bool CyclopsHeat_randomevent_disable = false;

        //ONLY APPLY When "Disable Random Event" is set to true
        //Increase the time that is needet to create a 
        [Slider("|- Multiply Time until meltdown", 1, 10, DefaultValue = 1,Tooltip = "Only Valid if Linar Meldown is selected")]
        public int CyclopsHeat_timetoplay = 1;

        //This slider increase the maximum Heat Temperatur the engine can have.
        //Warning this is NOT the Limit when a Fire occours.
        //For example this result in a longer cooldown you need to do AFTER a fire occurs.
        [Slider("Increase Max Heat Limit after Meltdown", 10, 50, DefaultValue = 10,Tooltip = "This Setting increase the absolut maximum Heat Limit the game will go to. This is not the Limit when the Fire start. For example. Increasing this Value will result in a highter amount of time you need to cool down the engine when you don´t stop the engine and continue driving")]
        public int CyclopsHeat_TempMaxHeat = 10;

        //Will increase the cooling effect per Tik when the temperatur outside the vessel is low
        [Toggle("Enable better cooling on low external Temp")]
        public bool CyclopsHeat_coolingontemp = false;

        //will increase the heating effect per Tik when the temperatur outside the vessel is high
        [Toggle("Enable faster heating on high external Temp")]
        public bool CyclopsHeat_fastheat = false;
    }
}
