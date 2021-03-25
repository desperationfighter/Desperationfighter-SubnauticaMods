using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace MorePathfinderNodes_BZ.Managment
{
    [Menu("MorePathfinderNodes_BZ", SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Slider("Available deployable Nodes", 10, 50, Step = 1, DefaultValue = 20, Tooltip = "[Default=20] Configure the Maximum Count of Node the Player can deploy.")]
        public int MaxNodes = 20;

        [Slider("Energy usage per Node", 0.05f, 1.0f, Step = 0.05f, DefaultValue = 0.5f, Format = "{0:F}", Tooltip = "[Default=0.5] Configure the Energy usage per Node deploy. (Recommend if you use a high number of Nodes)")]
        public float Energyusagepernode = 0.5f;
    }
}
