using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace ChargerWirelessCharging.Patches
{
    [HarmonyPatch(typeof(Player))]
    [HarmonyPatch(nameof(Player.FixedUpdate))]
    public static class Player_FixedUpdate_Patch
    {
        public static List<Battery> batteries;

        [HarmonyPostfix]
        private static void PostFix(Player __instance)
        {
            batteries = null;
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

                                    if (gameObject.TryGetComponent(out Battery intoolbattery))
                                    {
                                        batterieshighprio.Add(intoolbattery);
                                    }
                                
                            }

                        }
                    }
                    if (inventoryItem.item.gameObject.TryGetComponent(out Battery battery))
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
            while (keyValuePairs.MoveNext())
            {
                if (keyValuePairs.Current.Value != null)
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
        }
    }
}
