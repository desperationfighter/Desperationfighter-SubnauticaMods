using HarmonyLib;
using DAATQS_BZ.Managment;

namespace DAATQS_BZ.Patches
{
    [HarmonyPatch(typeof(QuickSlots))]
    [HarmonyPatch(nameof(QuickSlots.BindToEmpty))]
    public static class Quickslots_BindToEmpty_Patch
    {
        private static IngameConfigMenu ICM = new IngameConfigMenu();
        private static TechTypeAllowList TTAL = new TechTypeAllowList();

        [HarmonyPrefix]
        private static bool Prefix(QuickSlots __instance, InventoryItem item, ref int __result)
        {
            ICM.Load();

            int num = -1;
            for (int i = 0; i < __instance.binding.Length; i++)
            {
                if (__instance.binding[i] == null)
                {
                    num = i;
                    break;
                }
            }
            if (num == -1)
            {
                __result = -1;
            }


            if ((ICM.Config_ModEnable == false) | (PlayerAllowBind(item) && ICM.Config_AllowCustomList))
            //If the mod is disabled or check if the User allow adding
            {
                __instance.Bind(num, item);
            }
            __result = num;

            return false;
        }

        private static bool PlayerAllowBind(InventoryItem item)
        {
            TechType item_techtype = item.item.GetTechType();
            bool inlist = false;
            TTAL.Load();

            foreach (TechType Techtype_single in TTAL.TechType)
            {
                if (item_techtype == Techtype_single)
                {
                    inlist = true;
                }
            }

            if (inlist)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
