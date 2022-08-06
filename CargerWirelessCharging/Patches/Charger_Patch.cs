using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace CargerWirelessCharging.Patches
{
    [HarmonyPatch(typeof(Charger))]
    [HarmonyPatch(nameof(Charger.Awake))]
    public static class Charger_Awake_Patch
    {
        [HarmonyPostfix]
        private static void PostFix(Charger __instance)
        {
            //Charger_Update_Patch.wirelesschargertimer = 6f;
        }
    }


    [HarmonyPatch(typeof(Charger))]
    [HarmonyPatch(nameof(Charger.Update))]
    //[HarmonyPatch(typeof(BatteryCharger), nameof(Charger.Update))]
    public static class Charger_Update_Patch
    {
        public static float wirelesschargertimer;
        public static float wirelesschargertimerreset = 5f;
        internal static float chargeSpeed = 0.0035f;

        [HarmonyPostfix]
        //private static void PostFix(BatteryCharger __instance)
        private static void PostFix(Charger __instance)
        {           
            //BatteryCharger batteryCharger = __instance as BatteryCharger;
            //if (batteryCharger == null) return;
            //ErrorMessage.AddMessage("WirelessTest - Battery Charger");

            if (Player.main.currentSub == null) return;
            //ErrorMessage.AddMessage("WirelessTest - Player in Base");

            if (Time.deltaTime == 0f) return;

            if (wirelesschargertimer > 0f)
            {
                wirelesschargertimer -= DayNightCycle.main.deltaTime;
                if (wirelesschargertimer < 0f)
                {
                    wirelesschargertimer = 0f;
                }
            }
            bool charging = false;
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

                    List<TechType>invitemtypes = Inventory.main.container.GetItemTypes();
                    foreach (TechType t in invitemtypes)
                    {
                        if (!__instance.allowedTech.Contains(t))
                        {
                            ErrorMessage.AddMessage($"{t.ToString()} is not allowed");
                        }
                        else
                        {
                            IList<InventoryItem> inventoryItems = Inventory.main.container.GetItems(t); //Inventory.main.container.GetItems();
                            foreach (InventoryItem inventoryItem in inventoryItems)
                            {
                                if (inventoryItem == null)
                                {
                                    //ErrorMessage.AddMessage($"null");
                                }
                                else
                                {
                                    try
                                    {
                                        //ErrorMessage.AddMessage($"{CraftData.GetTechType(inventoryItem.item.gameObject)}");
                                        Battery battery = inventoryItem.item.gameObject.GetComponent<Battery>();
                                        if (battery != null)
                                        {
                                            //ErrorMessage.AddMessage($"identify bat ? {battery.name} - {battery.charge}");
                                            batteries.Add(battery);
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        }
                    }


                    //Charge Tool Prior
                    //GameObject storageRoot = Inventory.main?.storageRoot;
                    //Battery[] batteries = storageRoot.GetComponentsInChildren<Battery>(true);
                    foreach (Battery battery in batteries)
                    {
                        TechType found = CraftData.GetTechType(battery.gameObject);
                        string text = $"WirelessTest - Techtype {found.ToString()}";
                        //ErrorMessage.AddMessage(text);
                        //if (false) break;
                       
                        float charge = battery.charge;
                        float capacity = battery.capacity;
                        if (charge < capacity)
                        {
                            num++;
                            float num3 = DayNightCycle.main.deltaTime * chargeSpeed * capacity;
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
                    if (num2 > 0f && powerRelay.GetPower() > num2 && powerRelay.GetPower() > (powerRelay.GetMaxPower() * 0.3))
                    {
                        flag = true;
                        powerRelay.ConsumeEnergy(num2, out num4);
                    }
                    if (num4 > 0f)
                    {
                        charging = true;
                        float num5 = num4 / 4;
                        float charge2 = targetbattery.charge;
                        float capacity2 = targetbattery.capacity;
                        if (charge2 < capacity2)
                        {
                            float num6 = num5;
                            float num7 = capacity2 - charge2;
                            if (num6 > num7)
                            {
                                num6 = num7;
                            }
                            targetbattery.charge += num6;
                        }                        
                    }
                }
                if (num == 0 || !flag)
                {
                    wirelesschargertimer = wirelesschargertimerreset;
                }
            }
            if (charging)
            { 
                //ErrorMessage.AddMessage("WirelessTest - 1 - it was charged"); 
            }

            //---------------------------
            /*
            if (wirelesschargertimer <= 0f)
            {
                float chargeAmount = 0.004f * DayNightCycle.main.deltaTime;
                float energyconsumtion = 0.006f * DayNightCycle.main.deltaTime;
                PowerRelay powerRelay = PowerSource.FindRelay(__instance.transform);
                if (powerRelay != null && powerRelay.GetPower() > 25)
                {
                    BatteryCharger batteryCharger = __instance as BatteryCharger;
                    if (batteryCharger == null) return;
                    //ErrorMessage.AddMessage("WirelessTest - Battery Charger");

                    if (Player.main.currentSub == null) return;
                    ErrorMessage.AddMessage("WirelessTest - Player in Base");

                    //Charge Tool Prior
                    GameObject storageRoot = Inventory.main?.storageRoot;
                    Battery[] batteries = storageRoot.GetComponentsInChildren<Battery>(true);

                    bool toolsgotloaded = false;
                    foreach (Battery battery in batteries)
                    {
                        float missingCharge = battery.capacity - battery.charge;
                        if (missingCharge > 0)
                        {
                            float realcharge = chargeAmount * battery.capacity;
                            float amountGiven = realcharge;
                            if (missingCharge < realcharge)
                            {
                                amountGiven = missingCharge;
                            }
                            battery.charge += amountGiven;
                            float realenergyconsumtion = energyconsumtion * battery.capacity;
                            powerRelay.ConsumeEnergy(realenergyconsumtion, out float econsumtionbase);
                            ErrorMessage.AddMessage("WirelessTest - charged Tool");
                            toolsgotloaded = true;
                            break;
                            
                            //chargeAmount -= amountGiven;

                            //if (chargeAmount <= 0)
                            //{
                                //break;
                            //}
        }
                    }

                    if (!toolsgotloaded)
                    {
                        //Charge Batteries without Tools in the Inventory AFTER Tools are charged.
                        Inventory inventory = Inventory.Get();
                        foreach (var item in inventory.container)
                        {
                            if (item.item.TryGetComponent<IBattery>(out IBattery ibatteryComponent) && ibatteryComponent.charge < ibatteryComponent.capacity)
                            {
                                ErrorMessage.AddMessage("WirelessTest - charging battery");
                                ibatteryComponent.charge += chargeAmount * ibatteryComponent.capacity;
                                float realenergyconsumtion = energyconsumtion * ibatteryComponent.capacity;
                                powerRelay.ConsumeEnergy(realenergyconsumtion, out float econsumtionbase);
                                break;
                            }
                        }
                    }
                }
            }
            wirelesschargertimer = 6f;
            */
        }
    }
}
