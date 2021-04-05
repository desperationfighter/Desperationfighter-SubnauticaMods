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
            if (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.MetalHandsMK2TechType)
            {
               __instance.temperatureDamage.minDamageTemperature += 2f;
            }
        }
    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch(nameof(Player.HasReinforcedGloves))]
    public static class Player_HasReinforcedGloves
    {
        public static void Postfix(Player __instance, ref bool __result)
        {
            if (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.MetalHandsMK1TechType | Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.MetalHandsMK2TechType)
            {
                __result = true;
            }
        }
    }
}
