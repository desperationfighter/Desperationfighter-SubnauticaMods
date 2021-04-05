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
            //check if there is a empty slot
            int firstEmpty = __instance.GetFirstEmpty();
            if (firstEmpty == -1)
            {
                __result = -1;
            }
            else
            {
                ICM.Load();
                //Checklogic
                //1. Check if Mod is active
                //2. true if user use the custom list AND the item is found
                //check using before checking item because && will increase performance when mod is not in use
                //- 1 OR 2 musst be true
                if ( (ICM.Config_ModEnable == false) | (ICM.Config_AllowCustomList && PlayerAllowBind(item)) )
                {
                    __instance.Bind(firstEmpty, item);
                    __result = firstEmpty;
                }  
                else
                {
                    //As i cannot stop the execute on a patch methode give back a result seperatly to skip the Slot Select on the calling function 
                    __result = -1;
                }
            }
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
