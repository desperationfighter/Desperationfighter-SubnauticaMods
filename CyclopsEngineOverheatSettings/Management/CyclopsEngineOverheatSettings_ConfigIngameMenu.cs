using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace CyclopsEngineOverheatMonitor.Management
{
    [Menu("Cyclops Engine Overheat (Change Require restart)")]
    public class CyclopsEngineOverheatSettings_ConfigIngameMenu : ConfigFile
    {
        public CyclopsEngineOverheatSettings_ConfigIngameMenu() : base("config") { }

        [Toggle("Complete Heat Meachanic",Tooltip = "This option Disable the Complete Heat System! No Fire, No Warnings. Nothing will ever happen. Drive Flank Speed as long as you want")]
        public bool CyclopsHeat_generalmechanism = true;

        [Toggle("Fire on Overheat")]
        public bool CyclopsHeat_FireonOverheat = true;

        [Toggle("First Overheat Warning")]
        public bool CyclopsHeat_prewarning = true;

        [Toggle("First Overheat Damage possibility" , Tooltip = "At each warning there is already a Small chance for a Fire Breach Out. This will be disable this chance so no File will occour. WARNING DISABLING THIS AND THE MAIN WARNING WILL RESULT IN NO FIRE UNDER ANY CIRCUMSTACES")]
        public bool CyclopsHeat_damagefirst = true;

        [Toggle("Second Overheat Warning")]
        public bool CyclopsHeat_mainwarning = true;

        [Toggle("Second Overheat Damage possibility", Tooltip = "At each warning there is already a Small chance for a Fire Breach Out. This will be disable this chance so no File will occour. WARNING DISABLING THIS AND THE MAIN WARNING WILL RESULT IN NO FIRE UNDER ANY CIRCUMSTACES")]
        public bool CyclopsHeat_damagesecond = true;

        [Toggle("New Third Warning/Damage possibility",Tooltip = "This Creates a new Third Event where a Random Checker gets initiated with a extrem high chance for a Meltdown insteat of a moderate one on lower Tiers. (This Option can be used when Pre and Main Damage is not in used)")]
        public bool CyclopsHeat_createthirdchance = false;

        [Toggle("Make heat mechanic Linear instead Random",Tooltip = "Not recommend to use now. Because on a Mathematical Scale you get sooner a Meltdown to be directly after 10 Seconds (without Modifing cooling or heating)")]
        public bool CyclopsHeat_randomevent_disable = false;

        //ONLY APPLY When "Disable Random Event" is set to true
        [Slider("Multiply Time until Meltdown", 1, 10, DefaultValue = 1,Tooltip = "Only Valid if Linear Meltdown is selected")]
        public int CyclopsHeat_timetoplay = 1;

        //This slider increase the maximum Heat Temperatur the engine can have.
        //Warning this is NOT the Limit when a Fire occours.
        //For example this result in a longer cooldown you need to do AFTER a fire occurs.
        [Slider("Max absolut Heat Limit", 10, 50, DefaultValue = 10,Tooltip = "This Setting increase the absolut maximum Heat Limit the game will go to. This is not the Limit when the Fire start. For example. Increasing this Value will result in a highter amount of time you need to cool down the engine (after the fire start) when you don´t stop the Engine and continue driving")]
        public int CyclopsHeat_TempMaxHeat = 10;

        [Toggle("Double general Cooling Recover",Tooltip = "This options doubles the cooling effect. It applies for the Original Mechanic and the new and also and the low Temp Cooling Effect")]
        public bool CyclopsHeat_coolingrecover_double = false;

        //Will increase the cooling effect per Tik when the temperatur outside the vessel is low
        [Toggle("Enable better cooling on low external Temp")]
        public bool CyclopsHeat_fastcooling = false;

        //Will decrease the cooling effect per Tik when the temperatur outside the vessel is high
        [Toggle("Enable slower cooling on high external Temp")]
        public bool CyclopsHeat_slowcooling = false;

        //will increase the heating effect per Tik when the temperatur outside the vessel is high
        [Toggle("Enable faster heating on high external Temp")]
        public bool CyclopsHeat_fastheat = false;

        [Toggle("Enable slow heating on low external Temp")]
        public bool CyclopsHeat_slowheat = false;

        [Slider("External Temperatur Switch Warmwater", 40, 100, DefaultValue = 55 ,Tooltip = "Temperaturlimit when slow cooling and/or fast heating get activated")]
        public int CyclopsHeat_coolingrefertemp = 55;

        [Slider("External Temperatur Switch Coldwater", 10, 400, DefaultValue = 20, Tooltip = "Temperaturlimit when fast cooling and/or slow heating get activated")]
        public int CyclopsHeat_heatingrefertemp = 20;
    }
}
