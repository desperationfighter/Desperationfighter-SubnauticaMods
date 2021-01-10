using System;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using CyclopsEngineOverheatMonitor.Management;
//for Logging
using quickloger = QModManager.Utility;

namespace CyclopsEngineOverheatMonitor.Patches
{
    [HarmonyPatch(typeof(SubFire))]
	//[HarmonyPatch("EngineOverheatSimulation")]
	[HarmonyPatch(nameof(SubFire.EngineOverheatSimulation))]
	class Subfire_Patch
    {
		[HarmonyPrefix]
		private static bool PreFix(SubFire __instance)
		{
			//EngineOverheatSimulation(__instance, __originalMethod);
			EngineOverheatSimulation_Patch(__instance);
			return false;
		}

		private static CyclopsEngineOverheatSettings_ConfigIngameMenu CEO = new CyclopsEngineOverheatSettings_ConfigIngameMenu();

		//If somebody ask.....
		//yes i am very bad at algorithm, math and shit.....
		private static void EngineOverheatSimulation_Patch(SubFire __instance)
		{
			CEO.Load();
			
			if (!__instance.LOD.IsFull())
			{
				return;
			}

			if (CEO.CyclopsHeat_generalmechanism)
            {
				//--- Startup ----------------------------------------

				int makeitbigger = 3;
				int Warning_first = CEO.CyclopsHeat_aftertik_first * makeitbigger;
				int Warning_second = CEO.CyclopsHeat_aftertik_second * makeitbigger;
				int Warning_third = CEO.CyclopsHeat_aftertik_third * makeitbigger;
				int dmgchance_first = CEO.CyclopsHeat_dmgchance_first;
				int dmgchance_second = CEO.CyclopsHeat_dmgchance_second;
				int dmgchance_third = CEO.CyclopsHeat_dmgchance_third;
				int maxheatlimit = CEO.CyclopsHeat_TempMaxHeat * makeitbigger;
				int heatincreasepertik = 1 * makeitbigger;
				int cooling = 1 * makeitbigger;
				int lowspeedheat = 1;
				int lowspeedmax = CEO.CyclopsHeat_dmgchance_first - 1;

				float watertemp_float = WaterTemperatureSimulation.main.GetTemperature(__instance.subRoot.transform.position);
				int watertemp = (int)Math.Round(watertemp_float);

				//--- heating ----------------------------------------

				if (watertemp > CEO.CyclopsHeat_coolingrefertemp && CEO.CyclopsHeat_fastcooling == true)
				{
					heatincreasepertik = +2;
				}
				if (watertemp < CEO.CyclopsHeat_heatingrefertemp && CEO.CyclopsHeat_slowheat == true)
				{
					heatincreasepertik--;
				}

				if (__instance.subControl.cyclopsMotorMode.cyclopsMotorMode == CyclopsMotorMode.CyclopsMotorModes.Flank && __instance.subControl.appliedThrottle && __instance.cyclopsMotorMode.engineOn)
				{
					__instance.engineOverheatValue = Mathf.Min(__instance.engineOverheatValue + heatincreasepertik,maxheatlimit);
					int num = 0;

					if (__instance.engineOverheatValue > Warning_third && CEO.CyclopsHeat_createthirdchance == true)
					{
						num = UnityEngine.Random.Range(1, dmgchance_third);
						if (CEO.CyclopsHeat_warning_third)
						{
							__instance.subRoot.voiceNotificationManager.PlayVoiceNotification(__instance.subRoot.engineOverheatCriticalNotification, true, false);
						}
					}
					else if (__instance.engineOverheatValue > Warning_second)
					{
						if (CEO.CyclopsHeat_damagesecond)
						{
							num = UnityEngine.Random.Range(1, dmgchance_second);
						}
						if (CEO.CyclopsHeat_mainwarning)
						{
							__instance.subRoot.voiceNotificationManager.PlayVoiceNotification(__instance.subRoot.engineOverheatCriticalNotification, true, false);
						}
					}
					else if (__instance.engineOverheatValue > Warning_first)
					{
						if (CEO.CyclopsHeat_damagefirst)
						{
							num = UnityEngine.Random.Range(1, dmgchance_first);
						}
						if(CEO.CyclopsHeat_prewarning)
						{
							__instance.subRoot.voiceNotificationManager.PlayVoiceNotification(__instance.subRoot.engineOverheatNotification, true, false);
						}
					}

					if ( (num == 1 && CEO.CyclopsHeat_randomevent_disable == false) | (CEO.CyclopsHeat_randomevent_disable == true && __instance.engineOverheatValue > Warning_third) )
					{
						if(CEO.CyclopsHeat_FireonOverheat)
                        {
							__instance.CreateFire(__instance.roomFires[CyclopsRooms.EngineRoom]);
						}
						return;
					}
				}
				else
				{
					if(CEO.CyclopsHeat_standardspeedheating && __instance.subControl.cyclopsMotorMode.cyclopsMotorMode == CyclopsMotorMode.CyclopsMotorModes.Standard && __instance.subControl.appliedThrottle && __instance.cyclopsMotorMode.engineOn)
                    {
						if(__instance.engineOverheatValue < lowspeedmax)
                        {
							__instance.engineOverheatValue =+ lowspeedheat;
                        }
					}

					if (watertemp < CEO.CyclopsHeat_heatingrefertemp && CEO.CyclopsHeat_fastheat == true)
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
