using HarmonyLib;

namespace Snowfoxcloak.Patch
{
    [HarmonyPatch(typeof(Hoverbike))]
    [HarmonyPatch(nameof(Hoverbike.OnUpgradeModuleChange))]
    class Hoverbike_Patch
    {
        [HarmonyPostfix]
        public static void postfix(Hoverbike __instance, int slotID, TechType techType, bool added)
        {
            if (techType == Snowfoxcloak.SnowfoxCloakModuleTechType)
            {
                __instance.IceWormReductionModuleActive = added;
            }          
        }
    }
}
