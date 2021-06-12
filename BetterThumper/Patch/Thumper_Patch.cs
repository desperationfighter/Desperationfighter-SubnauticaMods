using HarmonyLib;
using System;

namespace BetterThumper.Patch
{
    [HarmonyPatch(typeof(Thumper))]
    [HarmonyPatch(nameof(Thumper.Start))]
    public static class Thumper_Patch_Start
    {
        [HarmonyPostfix]
        private static void Postfix(Thumper __instance)
        {
            __instance.energyPerSecond = BetterThumper.Config.energyPerSecond;
            __instance.thumperEffectRadius = BetterThumper.Config.thumperEffectRadius;
        }
    }

    [HarmonyPatch(typeof(Thumper))]
    [HarmonyPatch(nameof(Thumper.Update))]
    public static class Thumper_Patch_Update
    {
        [HarmonyPostfix]
        private static void Postfix(Thumper __instance)
        {
            __instance.energyPerSecond = BetterThumper.Config.energyPerSecond;
            __instance.thumperEffectRadius = BetterThumper.Config.thumperEffectRadius;
        }
    }
}
