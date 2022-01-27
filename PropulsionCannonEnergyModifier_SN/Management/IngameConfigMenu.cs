using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace PropulsionCannonEnergyModifier_SN.Management
{
    [Menu("PropulsionCannonEnergyModifier", SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Slider("Energy Usage", 0.05f, 2f, Step = 0.05f, DefaultValue = 0.5f, Format = "{0:F}", Tooltip = "[Default=0.5 / Vanilla=0.7] Changes the Battery Drain on usage.")]
        public float energyPerSecond = 0.5f;
    }
}