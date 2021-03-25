using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace MorePathfinderNodes.Managment
{
    [Menu("MorePathfinderNodes", SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Slider("Available Nodes", 10, 50, Step = 1, DefaultValue = 20, Tooltip = "Configure the Counts of Node the Player can deploy")]
        public int MaxNodes = 20;

        //[Slider("Energy Usage per Node", 0.0f, 1.0f, Step = 0.1f, DefaultValue = 0.5f, Tooltip = "Configure the Counts of Node the Player can deploy")]
        //public float Energyusagepernode = 0.5f;

        [Slider("Energy Usage per Node", 0.0f, 1.0f, Step = 0.1f, DefaultValue = 0.5f, Format = "{0:F1}", Tooltip = "Configure the Counts of Node the Player can deploy")]
        public float Energyusagepernode = 0.5f;

        [Slider("Energy Usage per Node (Value/10)", 0, 10, Step = 1, DefaultValue = 5, Tooltip = "Configure the Counts of Node the Player can deploy")]
        public float Energyusagepernode_skaler = 5;
    }
}
