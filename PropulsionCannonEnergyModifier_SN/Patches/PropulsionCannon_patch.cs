using HarmonyLib;
using UnityEngine;

namespace PropulsionCannonEnergyModifier_SN.Patches
{
    [HarmonyPatch(typeof(PropulsionCannon))]
    [HarmonyPatch(nameof(PropulsionCannon.Update))]
    public static class PropulsionCannon_patch_Update
    {
        [HarmonyPrefix]
        private static bool Prefix(PropulsionCannon __instance)
        {
            Prefix_patcher(__instance);
            return true;
        }

        private static void Prefix_patcher(PropulsionCannon __instance)
        {
            if (__instance.grabbedObject != null)
            {
                if (__instance.grabbedObject.GetComponent<Rigidbody>() != null)
                {
                    for (int i = 0; i < __instance.elecLines.Length; i++)
                    {
                        VFXElectricLine vfxelectricLine = __instance.elecLines[i];
                        vfxelectricLine.origin = __instance.muzzle.position;
                        vfxelectricLine.target = __instance.grabbedObjectCenter;
                        vfxelectricLine.originVector = __instance.muzzle.forward;
                    }
                }
                //__instance.energyInterface.ConsumeEnergy(Time.deltaTime * 0.7f);
                __instance.energyInterface.ConsumeEnergy(Time.deltaTime * PropulsionCannonEnergyModifier_SN.Config.energyPerSecond);
            }
            if (__instance.firstUseGrabbedObject != null)
            {
                for (int j = 0; j < __instance.elecLines.Length; j++)
                {
                    VFXElectricLine vfxelectricLine2 = __instance.elecLines[j];
                    vfxelectricLine2.origin = __instance.muzzle.position;
                    vfxelectricLine2.target = __instance.firstUseGrabbedObject.transform.position;
                    vfxelectricLine2.originVector = __instance.muzzle.forward;
                }
            }
        }
    }
}