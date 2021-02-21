using HarmonyLib;
using MetalHands.Managment;
using MetalHands.Items;

namespace MetalHands.Patches
{
    [HarmonyPatch(typeof(BreakableResource))]
    [HarmonyPatch(nameof(BreakableResource.HitResource))]
    public class BreakableResource_Patch
    {
        private static IngameConfigMenu ICM = new IngameConfigMenu();

        [HarmonyPrefix]
        private static bool Prefix(BreakableResource __instance)
        {
            ICM.Load();
            HitResource_Patch(__instance);
            return false;
        }

        private static void HitResource_Patch(BreakableResource __instance)
        {
            if (ICM.Config_ModEnable == true && Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveBlueprintTechType )
            //if (ICM.Config_ModEnable == true)
            {
                __instance.hitsToBreak = 0;
            }
            else
            {
                __instance.hitsToBreak--;
            }

            if (__instance.hitsToBreak == 0)
            {
                __instance.BreakIntoResources();
            }
        }
    }
}
