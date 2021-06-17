using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace BetterThumper.Managment
{
    [Menu("BetterThumper", SaveOn = MenuAttribute.SaveEvents.ChangeValue)]
    public class IngameConfigMenu : ConfigFile
    {
        public IngameConfigMenu() : base("config") { }

        [Slider("Energy Usage", 0.000f, 0.250f, Step = 0.025f, DefaultValue = 0.125f, Format = "{0:F}", Tooltip = "[Default=0.075 / Vanilla=0.125] Changes the Battery Drain on usage.")]
        public float energyPerSecond = 0.075f;

        [Slider("Safe Range", 30f, 180f, Step = 5f, DefaultValue = 90f, Tooltip = "[Default=90 / Vanilla=~60] Change the Safe Radius that the Thumper provide")]
        public float thumperEffectRadius = 90f;
    }
}