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
            /*
            //Do not need that anymore because we are going to patch HasReinforcedGlove and get Temperature Protection from the Original
            if (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveBlueprintTechType)
            {
                __instance.temperatureDamage.minDamageTemperature += 3f;
            }

            if (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType)
            {
               __instance.temperatureDamage.minDamageTemperature += 8f;
            }
            */

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
