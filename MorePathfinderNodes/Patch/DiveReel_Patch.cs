using HarmonyLib;

namespace MorePathfinderNodes.Patch
{
    
    [HarmonyPatch(typeof(DiveReel))]
    [HarmonyPatch(nameof(DiveReel.Start))]
    public static class DiveReel_maxNodes_Patch
    {
        [HarmonyPostfix]
        private static void PostFix(DiveReel __instance)
        {
            __instance.maxNodes = MorePathfinderNodesCore.Config.MaxNodes;
        }
    }

    /*
    [HarmonyPatch(typeof(DiveReel))]
    //[HarmonyPatch(nameof(DiveReel.maxNodes))]
    public static class DiveReel_maxNodes_Patch
    {
        [HarmonyPostfix]
        private static void PostFix(DiveReel __instance)
        {
            __instance.maxNodes = MorePathfinderNodesCore.Config.MaxNodes;
        }
    }
    */

    /*
    [HarmonyPatch(typeof(DiveReel))]
    [HarmonyPatch(nameof(DiveReel.energyCostPerDisc))]
    public static class DiveReel_energyCostPerDisc_Patch
    {
        [HarmonyPostfix]
        private static void PostFix(DiveReel __instance)
        {
            __instance.energyCostPerDisc = MorePathfinderNodesCore.Config.Energyusagepernode;
        }
    }
    */
}
