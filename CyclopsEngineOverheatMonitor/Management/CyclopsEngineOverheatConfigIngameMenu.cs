using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;

namespace CyclopsEngineOverheatMonitor.Management
{
    [Menu("Cyclops Engine Over Heat Monitor")]
    public class CyclopsEngineOverheatConfigIngameMenu : ConfigFile
    {
        public CyclopsEngineOverheatConfigIngameMenu() : base("config") { }

        [Toggle("1. Disable Complete Heat Meachanic")]
        public bool CyclopsHeat_general_disable = false;

        [Toggle("2. Disable First Overheat Warning")]
        public bool CyclopsHeat_prewarning_disable = false;

        [Toggle("2. Disable Second Overheat Warning")]
        public bool CyclopsHeat_mainwarning_disable = false;

        [Toggle("2. Disable Fire on Overheat")]
        public bool CyclopsHeat_FireonOverheat_disable = false;

        [Toggle("2. Double Cooling Recover")]
        public bool CyclopsHeat_coolingrecover_double = false;

        //This slider increase the maximum Heat Temperatur that get saved.
        //This result in a longer cooldown after a fired occurs
        [Slider("2. Temperatur above Max Heat", 10, 50, DefaultValue = 10)]
        public int CyclopsHeat_TempMaxHeat = 10;

        [Toggle("2. Enable better cooling on low Temp")]
        public bool CyclopsHeat_coolingontemp = false;

        [Toggle("2. Enable faster heating on high Temp")]
        public bool CyclopsHeat_fastheat = false;

        /*
        [Slider("My slider", 0, 50, DefaultValue = 25)]
        public int SliderValue = 25;
        [Toggle("My checkbox"), OnChange(nameof(MyCheckboxToggleEvent))]
        public bool ToggleValue;

        private void MyCheckboxToggleEvent(ToggleChangedEventArgs e)
        {

        }
        */
    }
}
