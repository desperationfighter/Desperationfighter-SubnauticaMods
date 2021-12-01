using HarmonyLib;

namespace PickupInventoryInfo_Nitrox.Patches
{
    [HarmonyPatch(typeof(BreakableResource))]
    [HarmonyPatch(nameof(BreakableResource.OnHandHover))]
    public static class BreakableResource_Patch
    {
        [HarmonyPostfix]
        private static void Postfix(BreakableResource __instance)
        {
            HandReticle.main.SetInteractText(__instance.breakText, Management.SharedLib.GetInventoryleftspace());
            if (!(Player.main.HasInventoryRoom(1, 1)))
            {              
                HandReticle.main.SetIcon(HandReticle.IconType.HandDeny, 1f);
            }
        }
    }
}
