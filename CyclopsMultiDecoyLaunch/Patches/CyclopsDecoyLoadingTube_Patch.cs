using HarmonyLib;
using CyclopsMultiDecoyLaunch.Management;

namespace CyclopsMultiDecoyLaunch.Patches
{
    [HarmonyPatch(typeof(CyclopsDecoyLoadingTube))]
    [HarmonyPatch(nameof(CyclopsDecoyLoadingTube.UnlockDefaultModuleSlots))]
    public static class CyclopsDecoyLoadingTube_Patch
    {
        private static bool Prefix(CyclopsDecoyLoadingTube __instance)
        {
            string[] array = new string[]
{
            "DecoySlot1",
            "DecoySlot2",
            "DecoySlot3",
            "DecoySlot4",
            "DecoySlot5",
            "DecoySlot6",
            "DecoySlot7",
            "DecoySlot8",
            "DecoySlot9",
            "DecoySlot10",
            "DecoySlot11",
            "DecoySlot12",
            "DecoySlot13",
            "DecoySlot14",
            "DecoySlot15"
};
            int num = 1;
            if (__instance.subRoot.decoyTubeSizeIncreaseUpgrade)
            {
                num = __instance.decoyManager.decoyMaxWithUpgrade;
            }
            else
            {
                for (int i = 1; i < array.Length; i++)
                {
                    __instance.decoySlots.RemoveSlot(array[i]);
                }
            }
            for (int j = 0; j < num; j++)
            {
                __instance.decoySlots.AddSlot(array[j]);
            }
            __instance.decoyManager.decoyMax = num;

            return false;
        }
    }
}
