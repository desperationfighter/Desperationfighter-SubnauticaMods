using HarmonyLib;

namespace DAATQS_BZ.Patches
{
    [HarmonyPatch(typeof(QuickSlots))]
    [HarmonyPatch(nameof(QuickSlots.BindToEmpty))]
    public static class Quickslots_BindToEmpty_Patch
    {
        [HarmonyPrefix]
        private static bool Prefix(QuickSlots __instance)
        {
            BindToEmpty_Patch(__instance);
            return false;
        }

        private static int BindToEmpty_Patch(QuickSlots __instance)
        {
                return -1;
        }
    }

    [HarmonyPatch(typeof(QuickSlots))]
    [HarmonyPatch(nameof(QuickSlots.Assign))]
    public static class Quickslots_Assign_Patch
    {
        [HarmonyPrefix]
        private static bool Prefix(QuickSlots __instance)
        {
            Assign_Patch(__instance);
            return false;
        }

        private static void Assign_Patch(QuickSlots __instance)
        {
			
		}
    }

    [HarmonyPatch(typeof(QuickSlots))]
    [HarmonyPatch(nameof(QuickSlots.OnAddItem))]
    public static class Quickslots_OnAddItem_Patch
    {
        [HarmonyPrefix]
        private static bool Prefix(QuickSlots __instance)
        {
            OnAddItem_Patch(__instance);
            return false;
        }

        private static void OnAddItem_Patch(QuickSlots __instance)
        {

        }
    }
}
