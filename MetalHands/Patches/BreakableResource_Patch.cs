using HarmonyLib;
using MetalHands.Managment;
using MetalHands.Items;
using UnityEngine;
//for Logging
using QModManager.API;
using QModManager.Utility;

namespace MetalHands.Patches
{
    //Utils.GetLocalPlayerComp().GetInMechMode()

    [HarmonyPatch(typeof(BreakableResource))]
    [HarmonyPatch(nameof(BreakableResource.HitResource))]
    public static class BreakableResource_HitResource_Patch
    {
        [HarmonyPrefix]
        private static bool Prefix(BreakableResource __instance)
        {
            MetalHands.Config.Load();
            HitResource_Patch(__instance);
            return false;
        }

        private static void HitResource_Patch(BreakableResource __instance)
        {
            if (MetalHands.Config.Config_ModEnable == true && ( ( Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveBlueprintTechType ) | (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType)) )
            {
                __instance.hitsToBreak = 0;
            }
            else
            {
                __instance.hitsToBreak--;
            }

            if (__instance.hitsToBreak == 0)
            {
                __instance.BreakIntoResources();
            }
        }
    }

    [HarmonyPatch(typeof(BreakableResource))]
    [HarmonyPatch(nameof(BreakableResource.BreakIntoResources))]
    public static class BreakableResource_BreakIntoResources_Patch
    {

        [HarmonyPrefix]
        private static bool Prefix(BreakableResource __instance)
        {
            MetalHands.Config.Load();
            BreakIntoResources_Patch(__instance);
            return false;
        }

        private static void BreakIntoResources_Patch(BreakableResource __instance)
        {
            __instance.SendMessage("OnBreakResource", null, SendMessageOptions.DontRequireReceiver);
            if (__instance.gameObject.GetComponent<VFXBurstModel>())
            {
                __instance.gameObject.BroadcastMessage("OnKill");
            }
            else
            {
                UnityEngine.Object.Destroy(__instance.gameObject);
            }
            if (__instance.customGoalText != "")
            {
                GoalManager.main.OnCustomGoalEvent(__instance.customGoalText);
            }
            bool flag = false;
            for (int i = 0; i < __instance.numChances; i++)
            {
                GameObject gameObject = __instance.ChooseRandomResource();
                if (gameObject)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "1 - Random Resouce is called");
                    ErrorMessage.AddMessage("1 - Random Resouce is called");
                    if (Player.main.GetVehicle() is Exosuit exosuit)
                    {
                        QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "2 - Start AddToPrawn over randomress");
                        ErrorMessage.AddMessage("2 - Start AddToPrawn over randomress");
                        AddtoPrawn(__instance, exosuit, gameObject);
                    }
                    else
                    {
                        if ( (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType) )
                        {
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "6 - Player has glove - randomress");
                            ErrorMessage.AddMessage("6 - Player has glove - randomress");
                            CraftData.AddToInventory(CraftData.GetTechType(gameObject));
                        }
                        else
                        {
                            __instance.SpawnResourceFromPrefab(gameObject);
                        }
                    }           
                    flag = true;
                }
            }
            if (!flag)
            {
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "3 - default resouce is called");
                ErrorMessage.AddMessage("3 - default resouce is called");
                if (Player.main.GetVehicle() is Exosuit exosuit)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "4 - Start AddToPrawn over defaultress");
                    ErrorMessage.AddMessage("4 - Start AddToPrawn over defaultress");
                    AddtoPrawn(__instance, exosuit, __instance.defaultPrefab);
                }
                else if(Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType )
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "7 - Player has glove - defaultress");
                    ErrorMessage.AddMessage("7 - Player has glove - defaultress");
                    CraftData.AddToInventory(CraftData.GetTechType(__instance.defaultPrefab));
                }
                else
                {
                    __instance.SpawnResourceFromPrefab(__instance.defaultPrefab);
                }
            }
            FMODUWE.PlayOneShot(__instance.breakSound, __instance.transform.position, 1f);
            if (__instance.hitFX)
            {
                Utils.PlayOneShotPS(__instance.breakFX, __instance.transform.position, Quaternion.Euler(new Vector3(270f, 0f, 0f)), null);
            }
        }

        private static void AddtoPrawn(BreakableResource __instance, Exosuit exosuit, GameObject gameObject)
        {
            //QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "10");
            //var pickupable = gameObject.GetComponent<Pickupable>();
            //QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "11");

            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "10");
            GameObject prefab = GameObject.Instantiate(gameObject);
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "11");
            var pickupable = prefab.GetComponent<Pickupable>();

            if (exosuit.storageContainer.container.HasRoomFor(pickupable))
            {
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "12");
                pickupable = pickupable.Initialize();
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "13");
                var item = new InventoryItem(pickupable);
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "14");
                exosuit.storageContainer.container.UnsafeAdd(item);
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "15");
                pickupable.Deactivate();
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "16");
            }
        }

        /*
        private static void AddtoPrawn(BreakableResource __instance,Exosuit exosuit, GameObject gameObject)
        {
            Pickupable pickupable = gameObject.GetComponent<Pickupable>();
            pickupable = pickupable.Initialize();

            if (exosuit.storageContainer.container.HasRoomFor(pickupable))           
            {
                //Main
                InventoryItem item = pickupable.inventoryItem;
                exosuit.storageContainer.container.UnsafeAdd(item);

                //old test
                //exosuit.storageContainer.container.AddItem(pickupable);

                //ItemsContainer.ItemGroup icig;
                //exosuit.storageContainer.container._items.TryGetValue(CraftData.GetTechType(gameObject), out icig);
                //exosuit.storageContainer.container._items.Add(CraftData.GetTechType(gameObject), icig);

            }
            else
            {
                __instance.SpawnResourceFromPrefab(gameObject);
            }
        }
        */
    }
}


//---------------------------------------------------------------------------------------------------------
//Vehicle vehicle = Player.main.GetVehicle();
//Exosuit exosuit = Player.main.currentMountedVehicle as Exosuit;
//Exosuit exosuit = Player.main.GetVehicle() as Exosuit;
//QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, exosuit.GetName() ); 
//if( Utils.GetLocalPlayerComp().GetInMechMode() )
//if ( Utils.GetLocalPlayerComp().IsPiloting() )
//if ( exosuit.CanPilot() && exosuit.GetPilotingMode() )
//if (exosuit.GetPilotingMode())
//if (exosuit.CanPilot())
//if (exosuit.storageContainer.container.HasRoomFor(size.x,size.y))
//---------------------------------------------------------------------------------------------------------