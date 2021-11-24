using HarmonyLib;
//for Logging
using QModManager.Utility;

namespace Warnbeforebreak.Patches
{
    [HarmonyPatch(typeof(BreakableResource))]
    [HarmonyPatch(nameof(BreakableResource.OnHandHover))]
    public static class BreakableResource_Patch
    {
        [HarmonyPostfix]
        private static void Postfix(BreakableResource __instance)
        {
            Logger.Log(Logger.Level.Debug, "Breakresource - OnHandover - loaded");
            if ( ! (Player.main.HasInventoryRoom(1, 1) ) )
            {
                Logger.Log(Logger.Level.Debug, "Has NO Room");
                HandReticle.main.SetInteractText(__instance.breakText, "Inventory Full !");
                HandReticle.main.SetIcon(HandReticle.IconType.HandDeny, 1f);
            }
        }
    }
}