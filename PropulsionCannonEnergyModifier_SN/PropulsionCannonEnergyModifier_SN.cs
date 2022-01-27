//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//for Logging
using QModManager.Utility;

using SMLHelper.V2.Handlers;
using PropulsionCannonEnergyModifier_SN.Management;

namespace PropulsionCannonEnergyModifier_SN
{
    [QModCore]
    public static class PropulsionCannonEnergyModifier_SN
    {
        internal static IngameConfigMenu Config { get; private set; }

        [QModPatch]
        public static void PropulsionCannonEnergyModifier_SN_Initial()
        {
            Logger.Log(Logger.Level.Debug, "PropulsionCannonEnergyModifier_SN Initialization");

            Harmony harmony = new Harmony("PropulsionCannonEnergyModifier_SN");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Logger.Log(Logger.Level.Info, "PropulsionCannonEnergyModifier_SN Patched");

            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();
        }
    }
}
