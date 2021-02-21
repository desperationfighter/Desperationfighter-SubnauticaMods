using HarmonyLib;
using MetalHands.Managment;
using MetalHands.Items;

using SMLHelper.V2.Utility;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MetalHands.Patches
{
    [HarmonyPatch(typeof(Equipment))]
    [HarmonyPatch(nameof(Equipment.GetCount))]
    internal class Equipment_GetCount_Patch
    {
        private static List<TechType> substitutionTargets = new List<TechType>();

        [HarmonyPostfix]
        public static void Postfix(Equipment __instance, ref int __result, TechType techType)
        {
            if( (techType == TechType.ReinforcedGloves) && (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveBlueprintTechType) )
            {
                __result++;
            }
        }
    }
}