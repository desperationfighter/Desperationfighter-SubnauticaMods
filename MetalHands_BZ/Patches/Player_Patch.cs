using HarmonyLib;
using MetalHands.Managment;
using MetalHands.Items;

namespace MetalHands.Patches
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch(nameof(Player.UpdateReinforcedSuit))]
    public static class Player_UpdateReinforcedSuit_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Player __instance)
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
        [HarmonyPostfix]
        public static void Postfix(ref bool __result)
        {
            if (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.MetalHandsMK1TechType | Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.MetalHandsMK2TechType)
            {
                __result = true;
            }
        }
    }

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch(nameof(Player.CheckColdsuitGoal))]
    public static class Player_CheckColdsuitGoal_Patch
    {
        [HarmonyPostfix]
        public static void Postfix(Player __instance)
        {
            Equipment equipment = Inventory.main.equipment;
            if (equipment.GetTechTypeInSlot("Body") == TechType.ColdSuit && (equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.MetalHandsMK1TechType || equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.MetalHandsMK2TechType) && equipment.GetTechTypeInSlot("Head") == TechType.ColdSuitHelmet)
            {
                __instance.coldSuitEquippedGoal.Trigger();
            }
        }
    }
}
