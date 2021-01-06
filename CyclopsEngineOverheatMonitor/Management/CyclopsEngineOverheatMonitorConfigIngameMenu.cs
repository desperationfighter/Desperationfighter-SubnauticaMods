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
    public class CyclopsEngineOverheatMonitorConfigIngameMenu : ConfigFile
    {
        [Toggle("My checkbox")]
        public bool CyclopsHeat_disable = false;

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
