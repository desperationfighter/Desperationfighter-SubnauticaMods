using UnityEngine;
using System.Collections.Generic;
using ChargerWirelessCharging.Patches;

namespace ChargerWirelessCharging.Mono
{
    public class WirelessChargerObject : MonoBehaviour
    {
        public SubRoot subRoot;
        public Charger charger;
        public static float wirelesschargertimer;
        public static float wirelesschargertimerreset = 5f;
        public bool basefound;

        public void Awake()
        {
            subRoot = GetComponentInParent<SubRoot>();
            charger = GetComponentInParent<Charger>();
            basefound = false;
        }

        public void FixedUpdate()
        {
            if (Player.main.currentSub == subRoot)
            {
                basefound = true;
            }
            else
            {
                basefound = false;
            }
        }

        public void Update()
        {
            if (!ChargerWirelessCharging.Config.Config_modEnable) return;
            if (!basefound) return;

            float distance = (Vector3.Distance(Player.main.gameObject.transform.position, charger.gameObject.transform.position));
            if (distance > ChargerWirelessCharging.Config.Config_maxPlayerDistanceToCharger)
            {
                return;
            }
            if (Time.deltaTime == 0f) return;
            if (wirelesschargertimer > 0f)
            {
                wirelesschargertimer -= DayNightCycle.main.deltaTime;
                if (wirelesschargertimer < 0f)
                {
                    wirelesschargertimer = 0f;
                }
            }
            if (wirelesschargertimer <= 0f)
            {
                int num = 0;
                bool flag = false;
                PowerRelay powerRelay = PowerSource.FindRelay(charger.transform);
                if (powerRelay != null)
                {
                    float num2 = 0f;
                    Battery targetbattery = null;

                    foreach (Battery battery in Player_FixedUpdate_Patch.batteries)
                    {
                        if (!charger.allowedTech.Contains(CraftData.GetTechType(battery.gameObject))) break;
                        float charge = battery.charge;
                        float capacity = battery.capacity;
                        if (charge < capacity)
                        {
                            num++;
                            float num3 = DayNightCycle.main.deltaTime * ChargerWirelessCharging.Config.Config_WirelessChargeSpeed * capacity;
                            if (charge + num3 > capacity)
                            {
                                num3 = capacity - charge;
                            }
                            num2 += num3;
                            //I only want to charge 1 Battery
                            targetbattery = battery;
                            break;
                        }
                    }
                    float num4 = 0f;
                    if (num2 > 0f && powerRelay.GetPower() > num2 && powerRelay.GetPower() > (powerRelay.GetMaxPower() / 3))
                    {
                        flag = true;
                        powerRelay.ConsumeEnergy(num2, out num4);
                    }
                    if (num4 > 0f)
                    {
                        float num5 = num4 / 4;
                        float charge2 = targetbattery.charge;
                        float capacity2 = targetbattery.capacity;
                        if (charge2 < capacity2)
                        {
                            float num6 = num5;
                            float num7 = capacity2 - charge2;
                            if (num7 < 0.005f)
                            {
                                targetbattery.charge = targetbattery.capacity;
                            }
                            else
                            {
                                if (num6 > num7)
                                {
                                    num6 = num7;
                                }
                                targetbattery.charge += num6;
                            }
                        }
                    }
                }
                if (num == 0 || !flag)
                {
                    wirelesschargertimer = wirelesschargertimerreset;
                }
            }
        }
    }
}
