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
            
            foreach (String Techtype_single in TTAL.TechType)
            {
                TechType Techtype_single_converted = TechTypeStuff.GetTechType(Techtype_single);
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
