using HarmonyLib;
//for Logging
using QModManager.Utility;

namespace Snowfoxcloak.Patch
{
    [HarmonyPatch(typeof(Hoverbike))]
    [HarmonyPatch(nameof(Hoverbike.OnUpgradeModuleChange))]
    class Hoverbike_Patch
    {
        [HarmonyPostfix]
        public static void postfix(Hoverbike __instance, int slotID, TechType techType, bool added)
        {
            Logger.Log(Logger.Level.Debug, "Hoverbike Postfix - running");
            if (techType == Snowfoxcloak.SnowfoxCloakModuleTechType)
            {
                Logger.Log(Logger.Level.Debug, "Hoverbike Postfix - Cloak module installed");
                __instance.IceWormReductionModuleActive = added;
            }
        }
    }
}
