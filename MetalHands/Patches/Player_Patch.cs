using HarmonyLib;

namespace MetalHands.Patches
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch(nameof(Player.UpdateReinforcedSuit))]
    public static class Player_UpdateReinforcedSuit_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Player __instance)
        {
            //additional Protection for the MK2
            if (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.MetalHandsMK2TechType)
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
            if (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.MetalHandsMK1TechType | Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.MetalHandsMK2TechType)
            {
                __result = true;
            }
        }
    }
}
