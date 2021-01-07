using System;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using CyclopsEngineOverheatMonitor.Management;

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
			//EngineOverheatSimulation(__instance, __originalMethod);
			EngineOverheatSimulation_Patch(__instance, __originalMethod);
			return false;
		}

		private static CyclopsEngineOverheatConfigIngameMenu CEO = new CyclopsEngineOverheatConfigIngameMenu();

		//If somebody ask.....
		//yes iam very bad at algorithm and shit.....
		private static void EngineOverheatSimulation_Patch(SubFire __instance, MethodBase __originalMethod)
		{
			if (!__instance.LOD.IsFull())
			{
				return;
			}

			int makeitbigger = 3;
			int maxheatlimit = CEO.CyclopsHeat_TempMaxHeat * makeitbigger;			
			int heatincreasepertik = 1 * makeitbigger;
			int cooling = 1 * makeitbigger;

			float watertemp_float = WaterTemperatureSimulation.main.GetTemperature(__instance.subRoot.transform.position);
			int watertemp = (int)Math.Round(watertemp_float);

			if (watertemp > CEO.CyclopsHeat_coolingrefertemp && CEO.CyclopsHeat_fastcooling == true)
            {
				heatincreasepertik=+2;
            }
			if(watertemp < 20 && CEO.CyclopsHeat_slowheat == true)
            {
				heatincreasepertik--;
            }

			if (CEO.CyclopsHeat_general_disable == false)
            {
				if (__instance.subControl.cyclopsMotorMode.cyclopsMotorMode == CyclopsMotorMode.CyclopsMotorModes.Flank && __instance.subControl.appliedThrottle && __instance.cyclopsMotorMode.engineOn)
				{
					__instance.engineOverheatValue = Mathf.Min(__instance.engineOverheatValue + heatincreasepertik,maxheatlimit);
					int num = 0;

					if (__instance.engineOverheatValue > (8 * makeitbigger) && CEO.CyclopsHeat_createthirdchance == true)
					{
						if (CEO.CyclopsHeat_randomevent_disable == true)
						{
							num = UnityEngine.Random.Range(1, 2);
						}
					}
					else if (__instance.engineOverheatValue > (5*makeitbigger))
					{
						if (CEO.CyclopsHeat_randomevent_disable == true)
						{
							num = UnityEngine.Random.Range(1, 4);
						}
						if (CEO.CyclopsHeat_mainwarning_disable == false)
						{
							__instance.subRoot.voiceNotificationManager.PlayVoiceNotification(__instance.subRoot.engineOverheatCriticalNotification, true, false);
						}
					}
					else if (__instance.engineOverheatValue > (3*makeitbigger))
					{
						if (CEO.CyclopsHeat_randomevent_disable == true)
						{
							num = UnityEngine.Random.Range(1, 6);
						}
						if(CEO.CyclopsHeat_prewarning_disable == false)
						{
							__instance.subRoot.voiceNotificationManager.PlayVoiceNotification(__instance.subRoot.engineOverheatNotification, true, false);
						}
					}

					if (num == 1 | (CEO.CyclopsHeat_randomevent_disable == true && __instance.engineOverheatValue > (8 * makeitbigger)) )
					{
						if(CEO.CyclopsHeat_FireonOverheat_disable == false)
                        {
							__instance.CreateFire(__instance.roomFires[CyclopsRooms.EngineRoom]);
						}
						return;
					}
				}
				else
				{
					
					if(watertemp < CEO.CyclopsHeat_heatingrefertemp && CEO.CyclopsHeat_fastheat == true)
                    {
						cooling=+2;
                    }
					if(watertemp > CEO.CyclopsHeat_coolingrefertemp && CEO.CyclopsHeat_slowheat == true)
                    {
						cooling--;
                    }
					if(CEO.CyclopsHeat_coolingrecover_double)
                    {
						cooling = cooling * 2;
					}
					
					if (__instance.subControl.cyclopsMotorMode.cyclopsMotorMode == CyclopsMotorMode.CyclopsMotorModes.Flank)
					{
						__instance.engineOverheatValue = Mathf.Max(1, __instance.engineOverheatValue - cooling);
						return;
					}
					__instance.engineOverheatValue = Mathf.Max(0, __instance.engineOverheatValue - cooling);
				}
			}
		} //EDNE private static void EngineOverheatSimulation_Patch

		private static void EngineOverheatSimulation(SubFire __instance, MethodBase __originalMethod)
        {
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
//-------------------------------------------------------------------------
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
//-------------------------------------------------------------------------
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

		}

	} // ende Class
} //ende Namespace
