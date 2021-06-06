using HarmonyLib;
using DAATQS.Managment;
using System;

namespace DAATQS.Patches
{
    //Entfernt das binden an die Bar
    //Solange andere Funktionen darauf zugreifen muss es ein Return geben.

    //This removed the binding to QQuickslots
    //As long other Functions are in Contact to this we need to Return a Value to prevent an Error. So in worst Case i give back a "-1".
    [HarmonyPatch(typeof(QuickSlots))]
    [HarmonyPatch(nameof(QuickSlots.BindToEmpty))]
    public static class Quickslots_BindToEmpty_Patch
    {
        private static IngameConfigMenu ICM = new IngameConfigMenu();
        private static TechTypeAllowList TTAL = new TechTypeAllowList();

        [HarmonyPrefix]
        private static bool Prefix(QuickSlots __instance, InventoryItem item, ref int __result)
        {
            //ICM.Load();

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
                __result = - 1;
            }

            /*
            //Check if intro is running
            if(IntroVignette.isIntroActive == true)
            {
                Logger.Log(Logger.Level.Warn, "Intro true");
            }
            else
            {
                Logger.Log(Logger.Level.Warn, "Intro false");
            }
            */
            
            if ( ( ICM.Config_ModEnable == false ) | ( IntroVignette.isIntroActive == true ) | (ICM.Config_AllowCustomList && PlayerAllowBind(item)) )
                //If the mod is disabled or the intro is runing run the original Code. After all check if the User allow adding to improve performance check if Allow is enabled before checking the item.
            {
                __instance.Bind(num, item);
            }            
            __result = num;

            return false;
        }

        private static bool PlayerAllowBind(InventoryItem item)
        {
            //Lookup the Techtype of the object
            TechType item_techtype = item.item.GetTechType();
            bool inlist = false;
            //load the Allow List into "cache"
            TTAL.Load();
            
            foreach (String Techtype_single in TTAL.TechType)
            {
                //Convert the TechType String into a real Techtype this avoid conflict with modded items.
                TechType Techtype_single_converted = TechTypeStuff.GetTechType(Techtype_single);
                //Check if the Player allow the adding.
                if (item_techtype == Techtype_single_converted)
                {
                    inlist = true;
                }           
            }

            if ( inlist )
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

} // Namespace
