using HarmonyLib;
using CyclopsMultiDecoyLaunch.Management;

namespace CyclopsMultiDecoyLaunch.Patches
{
    [HarmonyPatch(typeof(CyclopsDecoyLaunchButton))]
    [HarmonyPatch(nameof(CyclopsDecoyLaunchButton.OnClick))]
    internal class CyclopsDecoyLaunchButton_OnClick_Patch
    {
        private static IngameConfigMenu ICM = new IngameConfigMenu();

        private static void Postfix(CyclopsDecoyLaunchButton __instance)
        {
            if(CyclopsMultiDecoyLaunch.iknowwhatido)
            {
                for (int i = 0; i < ICM.AdditionalDecoystodeploy; i++)
                {
                    if (Player.main.currentSub != __instance.subRoot)
                    {
                        return;
                    }
                    if (!__instance.decoyManager.TryLaunchDecoy())
                    {
                        return;
                    }
                }
            }
        }
    }
}
