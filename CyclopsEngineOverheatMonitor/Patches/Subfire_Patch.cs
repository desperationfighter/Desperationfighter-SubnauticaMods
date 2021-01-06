using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using UnityEngine;

using QModManager.API;

namespace CyclopsEngineOverheatMonitor.Patches
{
    [HarmonyPatch(typeof(SubFire))]
    [HarmonyPatch("EngineOverheatSimulation")]
    class Subfire_Patch
    {
		private static int engineOverheatValue_Patched;	
		public static int engineOverheatValue_Patched_pub { get; }

		[HarmonyPrefix]
		private static void PreFix(SubFire __instance)
        {
			EngineOverheatSimulation(__instance);
		}

		private static void EngineOverheatSimulation(SubFire __instance)
        {
			if (__instance.LOD.IsFull())
			{
				return;
			}
			if (__instance.cyclopsMotorMode.cyclopsMotorMode == CyclopsMotorMode.CyclopsMotorModes.Flank && __instance.subControl.appliedThrottle && __instance.cyclopsMotorMode.engineOn)
			{
				engineOverheatValue_Patched = Mathf.Min(engineOverheatValue_Patched + 1, 10);
				int num = 0;
				if (engineOverheatValue_Patched > 5)
				{
					num = UnityEngine.Random.Range(1, 4);
					__instance.subRoot.voiceNotificationManager.PlayVoiceNotification(__instance.subRoot.engineOverheatCriticalNotification, true, false);
					QModServices.Main.AddCriticalMessage("Überhitzung");
				}
				else if (engineOverheatValue_Patched > 3)
				{
					QModServices.Main.AddCriticalMessage("Kritisch");
					num = UnityEngine.Random.Range(1, 6);
					__instance.subRoot.voiceNotificationManager.PlayVoiceNotification(__instance.subRoot.engineOverheatNotification, true, false);
				}
				if (num == 1)
				{
					//__instance.CreateFire(__instance.roomFires[CyclopsRooms.EngineRoom]);
					QModServices.Main.AddCriticalMessage("Boom ein Feuer wäre ausgebrochen");
					return;
				}
			}
			else
			{
				if (__instance.cyclopsMotorMode.cyclopsMotorMode == CyclopsMotorMode.CyclopsMotorModes.Flank)
				{
					engineOverheatValue_Patched = Mathf.Max(1, engineOverheatValue_Patched - 1);
					return;
				}
				engineOverheatValue_Patched = Mathf.Max(0, engineOverheatValue_Patched - 1);
			}
		}
    }
}
