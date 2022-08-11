using ChargerWirelessCharging.Mono;
using HarmonyLib;

namespace ChargerWirelessCharging.Patches
{
    [HarmonyPatch(typeof(BatteryCharger))]
    [HarmonyPatch(nameof(BatteryCharger.Start))]
    public static class BatteryCharger_Patch
    {
        [HarmonyPostfix]
        private static void PostFix(BatteryCharger __instance)
        {
            //__instance.gameObject.AddComponent<WirelessChargerMonoObject>();
        }
    }
}
