using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace ChargerWirelessCharging.Patches
{
    [HarmonyPatch(typeof(Charger))]
    [HarmonyPatch(nameof(Charger.Update))]
    public static class Charger_Update_Patch
    {
        public static float wirelesschargertimer;
        public static float wirelesschargertimerreset = 5f;

        [HarmonyPostfix]
        private static void PostFix(Charger __instance)
        {
            if(!ChargerWirelessCharging.Config.Config_modEnable) return;

            //old simple test way
            //if (Player.main.currentSub == null) return;

            //new good way
            bool basefound = false;
            if (Player.main.currentSub != null)
            {
                //if(Player.main.currentSub.TryGetComponent<Base>(out Base baseplayercurrently))
                //{
                //    ErrorMessage.AddMessage("Base found where the player is");
                //    if (__instance.GetComponentInParent<Base>() == baseplayercurrently)
                //    {
                //        ErrorMessage.AddMessage("Player is in base of the Charger");
                //        basefound = true;
                //    }
                //    //else
                //    //{
                //        //ErrorMessage.AddMessage("Charger is in different base");
                //        return;
                //    //}
                //}

                if(__instance.GetComponentInParent<SubRoot>() == Player.main.currentSub)
                {
                    basefound = true;
                }
                else
                {
                    return;
                }
            }
            else
            {
                ErrorMessage.AddMessage("Player is in no base");
                return;
            }
            if (!basefound) return;

            float distance = (Vector3.Distance(Player.main.gameObject.transform.position, __instance.gameObject.transform.position));
            if (distance > ChargerWirelessCharging.Config.Config_maxPlayerDistanceToCharger)
            {
                //ErrorMessage.AddMessage($"distance is bigger {wirelesschargingdistance} -> {distance}");
                return;
            }
            //else
            //{
                //ErrorMessage.AddMessage($"{ wirelesschargingdistance} -> { distance}");
            //}

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
                PowerRelay powerRelay = PowerSource.FindRelay(__instance.transform);
                if (powerRelay != null)
                {
                    float num2 = 0f;
                    Battery targetbattery = null;
                    List<Battery> batteries = new List<Battery>();
                    List<Battery> batterieslowprio = new List<Battery>();
                    List<Battery> batterieshighprio = new List<Battery>();

                    List<TechType> invitemtypes = Inventory.main.container.GetItemTypes();
                    foreach (TechType t in invitemtypes)
                    {
                        IList<InventoryItem> inventoryItems = Inventory.main.container.GetItems(t);
                        foreach (InventoryItem inventoryItem in inventoryItems)
                        {
                            if (inventoryItem.item.gameObject.TryGetComponent(out EnergyMixin eminmax))
                            {
                                if (eminmax != null && eminmax.HasItem())
                                {
                                    GameObject gameObject = eminmax.GetBattery();
                                    //keep in Mind for Below Zero and Upcoming Experimental Branch of SN
                                    //GameObject gameObject = eminmax.GetBatteryGameObject();
                                    if (gameObject != null)
                                    {
                                        if (__instance.allowedTech.Contains(CraftData.GetTechType(gameObject)))
                                        {
                                            if (gameObject.TryGetComponent(out Battery intoolbattery))
                                            {
                                                batterieshighprio.Add(intoolbattery);
                                            }
                                        }
                                    }

                                }
                            }
                            if (__instance.allowedTech.Contains(CraftData.GetTechType(inventoryItem.item.gameObject)) && inventoryItem.item.gameObject.TryGetComponent(out Battery battery))
                            {
                                batterieslowprio.Add(battery);
                            }
                        }
                    }

                    foreach (Battery b_h in batterieshighprio)
                    {
                        batteries.Add(b_h);
                    }

                    bool playerHasChipEquipted = false;
                    Dictionary<string, InventoryItem>.Enumerator keyValuePairs = Inventory.main.equipment.GetEquipment();
                    while(keyValuePairs.MoveNext())
                    {
                        if(keyValuePairs.Current.Value != null)
                        {
                            InventoryItem inventoryItem = keyValuePairs.Current.Value;
                            if (inventoryItem.item.GetTechType() == ChargerWirelessCharging.WirelessChargerChipTechType)
                            {
                                playerHasChipEquipted = true;
                            }
                        }
                    }

                    //and here is the Workaround ..... ..... .... yeah .... well....
                    if (ChargerWirelessCharging.Config.Config_chargeLooseBatteryWithoutChip || playerHasChipEquipted)
                    //The easy way does not work with the MoreChipSlots Mod so yeah i need to find a workaround
                    //if (CargerWirelessCharging.Config.Config_chargeLooseBatteryWithoutChip | Inventory.main.equipment.GetTechTypeInSlot("Chip") == CargerWirelessCharging.WirelessChargerChipTechType)
                    {
                        foreach (Battery b_l in batterieslowprio)
                        {
                            batteries.Add(b_l);
                        }
                    }

                    foreach (Battery battery in batteries)
                    {                      
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
                    if (num2 > 0f && powerRelay.GetPower() > num2 && powerRelay.GetPower() > (powerRelay.GetMaxPower()/3) )
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
                            if(num7 < 0.005f)
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
