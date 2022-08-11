using UnityEngine;
using System.Collections.Generic;
using ChargerWirelessCharging.Patches;

namespace ChargerWirelessCharging.Mono
{
    public class WirelessChargerMonoObject : MonoBehaviour
    {
        public SubRoot subRoot;
        public Charger charger;
        public float wirelesschargertimer;
        public float wirelesschargertimerreset = 5f;
        public bool basefound;
        public float interrimdistance;

        public void Awake()
        {
            subRoot = GetComponentInParent<SubRoot>();
            charger = GetComponentInParent<Charger>();
            basefound = false;
            wirelesschargertimer = 0f;
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

            interrimdistance  = (Vector3.Distance(Player.main.gameObject.transform.position, charger.gameObject.transform.position));
        }

        public void Update()
        {
            if (!ChargerWirelessCharging.Config.Config_modEnable) return;
            if (!basefound) return;
            if (interrimdistance > ChargerWirelessCharging.Config.Config_maxPlayerDistanceToCharger) return;
            if (Time.deltaTime == 0f) return;

            float DNCdelta = DayNightCycle.main.deltaTime;


            if (wirelesschargertimer > 0f)
            {
                wirelesschargertimer -= DNCdelta;
                if (wirelesschargertimer < 0f)
                {
                    wirelesschargertimer = 0f;
                }
            }
            if (wirelesschargertimer <= 0f)
            {
                bool flag = false;
                Battery targetbattery = null;
                PowerRelay powerRelay = PowerSource.FindRelay(charger.transform);
                if (powerRelay != null)
                {
                    float num2 = 0f;
                    foreach (Battery battery in Player_BatteryStorage.batteries)
                    {
                        if (!charger.allowedTech.Contains(CraftData.GetTechType(battery.gameObject))) continue;
                        float charge = battery.charge;
                        float capacity = battery.capacity;
                        if (charge < capacity)
                        {
                            float num3 = DNCdelta * (charger.chargeSpeed/100*ChargerWirelessCharging.Config.Config_WirelessChargeSpeed) * capacity;
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
                        float num5 = num4;
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
                if (targetbattery == null || !flag)
                {
                    wirelesschargertimer = wirelesschargertimerreset;
                }
            }
        }
    }
}
