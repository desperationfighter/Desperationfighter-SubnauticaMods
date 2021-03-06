using HarmonyLib;
using MetalHands.Managment;
using MetalHands.Items;

namespace MetalHands.Patches
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch(nameof(Player.UpdateReinforcedSuit))]
    public class Player_UpdateReinforcedSuit_Patch
    {
        [HarmonyPostfix]
        static void Postfix(Player __instance)
        {
            if (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.GloveBlueprintTechType)
            {
                __instance.temperatureDamage.minDamageTemperature += 3f;
            }

            if (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.GloveMK2BlueprintTechType)
            {
               __instance.temperatureDamage.minDamageTemperature += 8f;
            }
        }
    }
}
