using HarmonyLib;
using CyclopsMultiDecoyLaunch.Management;

namespace CyclopsMultiDecoyLaunch.Patches
{
    [HarmonyPatch(typeof(CyclopsDecoyManager))]
    [HarmonyPatch(nameof(CyclopsDecoyManager.Start))]
    public static class CyclopsDecoyManager_Start_Patch
    {
        private static IngameConfigMenu ICM = new IngameConfigMenu();

        private static void Postfix(CyclopsDecoyManager __instance)
        {
            if(ICM.EnableOverrideMaxDecoyStorage)
            {
                __instance.decoyMax = ICM.OverrideMaxDecoyStorage;
            }
        }
    }

    [HarmonyPatch(typeof(CyclopsDecoyManager))]
    [HarmonyPatch(nameof(CyclopsDecoyManager.UpdateMax))]
    public static class CyclopsDecoyManager_UpdateMax_Patch
    {
        private static IngameConfigMenu ICM = new IngameConfigMenu();

        private static void Postfix(CyclopsDecoyManager __instance)
        {
            if (ICM.EnableOverrideMaxDecoyStorage)
            {
                __instance.decoyMax = ICM.OverrideMaxDecoyStorage;
            }
        }
    }
}