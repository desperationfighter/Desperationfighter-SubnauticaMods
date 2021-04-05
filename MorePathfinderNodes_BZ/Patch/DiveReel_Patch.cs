using HarmonyLib;

namespace MorePathfinderNodes_BZ.Patch
{
    
    [HarmonyPatch(typeof(DiveReel))]
    [HarmonyPatch(nameof(DiveReel.Start))]
    public static class DiveReel_maxNodes_Patch
    {
        [HarmonyPostfix]
        private static void PostFix(DiveReel __instance)
        {
            __instance.maxNodes = MorePathfinderNodesCore_BZ.Config.MaxNodes;
            __instance.energyCostPerDisc = MorePathfinderNodesCore_BZ.Config.Energyusagepernode;
        }
    }
}
