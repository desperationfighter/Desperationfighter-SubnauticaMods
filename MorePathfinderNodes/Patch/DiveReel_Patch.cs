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
            __instance.energyCostPerDisc = MorePathfinderNodesCore.Config.Energyusagepernode;
        }
    }
}
