/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
*/

using HarmonyLib;
using UnityEngine;

using QModManager.API;


	using MoreCyclopsUpgrades.API;
	using MoreCyclopsUpgrades.API.General;
	using MoreCyclopsUpgrades.API.Upgrades;
using System.Reflection;

namespace CyclopsEngineOverheatMonitor.Patches
{
    [HarmonyPatch(typeof(SubFire))]
	//[HarmonyPatch("EngineOverheatSimulation")]
	[HarmonyPatch(nameof(SubFire.EngineOverheatSimulation))]
	class Subfire_Patch
    {
		[HarmonyPrefix]
		private static bool PreFix(SubFire __instance,MethodBase __originalMethod)
		{
			EngineOverheatSimulation(__instance, __originalMethod);
			return false;
		}
		

		private static void EngineOverheatSimulation(SubFire __instance, MethodBase __originalMethod)
        {
			//QModServices.Main.AddCriticalMessage("Starte Simulation");
			if (!__instance.LOD.IsFull())
			{
				//QModServices.Main.AddCriticalMessage("LOD is Full");
				return;
			}

			/*
			if (__instance.subControl.cyclopsMotorMode.cyclopsMotorMode == CyclopsMotorMode.CyclopsMotorModes.Flank)
			{
				QModServices.Main.AddCriticalMessage("Flank aktiviert");
			}
			else if (__instance.subControl.cyclopsMotorMode.cyclopsMotorMode == CyclopsMotorMode.CyclopsMotorModes.Standard)
			{
				QModServices.Main.AddCriticalMessage("standart aktiviert");
			}
			else if (__instance.subControl.cyclopsMotorMode.cyclopsMotorMode == CyclopsMotorMode.CyclopsMotorModes.Slow)
			{
				QModServices.Main.AddCriticalMessage("Slow aktiviert");
			}
			else
			{
				QModServices.Main.AddCriticalMessage("geschwindigkeit gescheitert");
			}


			if (__instance.subControl.appliedThrottle == true)
            {
				QModServices.Main.AddCriticalMessage("Throttle true");
			}
			else if(__instance.subControl.appliedThrottle == false)
			{
				QModServices.Main.AddCriticalMessage("Throttle false");
			}
			else
            {
				QModServices.Main.AddCriticalMessage("Throttle Error");
			}

			if (__instance.cyclopsMotorMode.engineOn == true)
            {
				QModServices.Main.AddCriticalMessage("EngineOn");
			}
			else if (__instance.cyclopsMotorMode.engineOn == false)
			{
				QModServices.Main.AddCriticalMessage("EngineOff");
			}
			else
            {
				QModServices.Main.AddCriticalMessage("Engine Error");
			}
			*/
			
			if (__instance.subControl.cyclopsMotorMode.cyclopsMotorMode == CyclopsMotorMode.CyclopsMotorModes.Flank && __instance.subControl.appliedThrottle && __instance.cyclopsMotorMode.engineOn)
			{
				QModServices.Main.AddCriticalMessage("Flank aktiviert");

				__instance.engineOverheatValue = Mathf.Min(__instance.engineOverheatValue + 1, 10);
				int num = 0;
				if (__instance.engineOverheatValue > 5)
				{
					num = UnityEngine.Random.Range(1, 4);
					__instance.subRoot.voiceNotificationManager.PlayVoiceNotification(__instance.subRoot.engineOverheatCriticalNotification, true, false);
					QModServices.Main.AddCriticalMessage("Überhitzung");
				}
				else if (__instance.engineOverheatValue > 3)
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
				if (__instance.subControl.cyclopsMotorMode.cyclopsMotorMode == CyclopsMotorMode.CyclopsMotorModes.Flank)
				{
					__instance.engineOverheatValue = Mathf.Max(1, __instance.engineOverheatValue - 1);
					return;
				}
				__instance.engineOverheatValue = Mathf.Max(0, __instance.engineOverheatValue - 1);
			}
			
		}
    }
}
