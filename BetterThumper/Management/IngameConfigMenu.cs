using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace BetterThumper.Managment
{
    [Menu("BetterThumper", SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Slider("Energy Usage", 0.000f, 0.250f, Step = 0.025f, DefaultValue = 0.125f, Tooltip = "[Default=0.075  Vanilla=0.125] Changes the Battery Drain on usage.")]
        public float energyPerSecond = 0.075f;

        [Slider("Safe Range", 30f, 330f, Step = 15f, DefaultValue = 120f, Tooltip = "[Default=120 Vanilla=60] Change the Safe Radius that the Thumper provide")]
        public float thumperEffectRadius = 120f;
    }
}