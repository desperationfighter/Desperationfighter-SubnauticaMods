using HarmonyLib;
using DAATQS.Managment;
//BAsic function
using System;
//for Logging
using QModManager.Utility;

using System.Collections;


namespace DAATQS.Patches
{
    //Entfernt das binden an die Bar
    //Solange andere Funktionen darauf zugreifen muss es ein Return geben.
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
            
            if ( ( ICM.Config_ModEnable == false ) | ( IntroVignette.isIntroActive == true ) | ( PlayerAllowBind(item) && ICM.Config_AllowCustomList ) )
                //If the mod is disabled or the intro is runing run the original Code. After all check if the User allow adding
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

            //foreach (TechType techtype_single in (TechType[]) Enum.GetValues(typeof(TechType)))
            //{
            //}
            
            foreach (TechType Techtype_single in TTAL.TechType)
            {
                if (item_techtype == Techtype_single)
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
    } //public static class Quickslots_BindToEmpty_Patch
//-----------------------------------------------------------------------------------------------------*/


    //Wird nicht doch nicht benötigt
    /*
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
       } // public static class Quickslots_Assign_Patch
    */


    //Diese Funktion führt dazu, dass nach dem aufsammeln oder verschieben aus einem Schrank das Item  zur Quickslotbar hinzugefügt wird.
    //Das unterbrechen der funktion fü+hrt dazu, dass das Item nicht mehr hinzugefügt wird.
    //Nachteil: Durch das entfernen des Code gibt es ein Reload Problem.
    //Eigendlich ist das Entfernen der Funktion rendundant, wenn BindToEmpty bereits entfernt wird.

    /*
        [HarmonyPatch(typeof(QuickSlots))]
        [HarmonyPatch(nameof(QuickSlots.OnAddItem))]
        public static class Quickslots_OnAddItem_Patch
        {
            [HarmonyPrefix]
            private static bool Prefix(QuickSlots __instance, InventoryItem item)
            {
                OnAddItem_Patch(__instance, item);
                return false;
            }

            private static void OnAddItem_Patch(QuickSlots __instance, InventoryItem item)
            {

            }
        } //public static class Quickslots_OnAddItem_Patch
    */

} // Namespace
