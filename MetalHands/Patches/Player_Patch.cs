using HarmonyLib;
using MetalHands.Managment;
using MetalHands.Items;

namespace MetalHands.Patches
{
    //[HarmonyPatch(typeof(Player))]
    //[HarmonyPatch(nameof(Player.UpdateReinforcedSuit))]
    //public class Player_UpdateReinforcedSuit_Patch
    //{
    //    [HarmonyPostfix]
    //    static void Postfix(Player __instance)
    //    {
    //        if (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveBlueprintTechType)
    //        {
    //            __instance.temperatureDamage.minDamageTemperature += 8f;
    //        }
    //    }
    //}

    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch(nameof(Player.HasReinforcedGloves))]
    public class Player_HasReinforcedGloves_Patch
    {
        [HarmonyPostfix]
        static void Postfix(Player __instance, ref bool __result)
        {
            __result = Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveBlueprintTechType;
        }
    }
}
