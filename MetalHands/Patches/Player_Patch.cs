﻿using HarmonyLib;
using MetalHands.Managment;
using MetalHands.Items;

namespace MetalHands.Patches
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch(nameof(Player.UpdateReinforcedSuit))]
    public class Player_Patch
    {
        [HarmonyPostfix]
        static void Postfix(Player __instance)
        {
            if (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveBlueprintTechType)
            {
                __instance.temperatureDamage.minDamageTemperature += 8f;
            }
        }
    }
}
