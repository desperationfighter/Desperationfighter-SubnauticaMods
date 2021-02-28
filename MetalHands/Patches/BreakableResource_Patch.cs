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
        private static IngameConfigMenu ICM = new IngameConfigMenu();

        [HarmonyPrefix]
        private static bool Prefix(BreakableResource __instance)
        {
            ICM.Load();
            HitResource_Patch(__instance);
            return false;
        }

        private static void HitResource_Patch(BreakableResource __instance)
        {
            if (ICM.Config_ModEnable == true && ( ( Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveBlueprintTechType ) | (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType)) )
            //if (ICM.Config_ModEnable == true)
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
        private static IngameConfigMenu ICM = new IngameConfigMenu();

        [HarmonyPrefix]
        private static bool Prefix(BreakableResource __instance)
        {
            BreakIntoResources_Patch(__instance);
            return false;
        }

        private static void BreakIntoResources_Patch(BreakableResource __instance)
        {
            ICM.Load();
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
                    //Exosuit exosuit = Player.main.currentMountedVehicle as Exosuit;
                    Exosuit exosuit = Player.main.GetVehicle() as Exosuit;

                    //if( Utils.GetLocalPlayerComp().GetInMechMode() )
                    //if ( Utils.GetLocalPlayerComp().IsPiloting() )
                    //if ( exosuit.CanPilot() && exosuit.GetPilotingMode() )
                    if (exosuit.GetPilotingMode())
                    {
                        QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Is Piloting");
                        QModServices.Main.AddCriticalMessage("Is Piloting");
                        //Vehicle vehicle = Player.main.GetVehicle();
                        
                        Pickupable pickupable = gameObject.GetComponent<Pickupable>();
                        
                        if( !(exosuit == null) )
                        {
                            QModServices.Main.AddCriticalMessage("vehiocle found");
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Vehicle Found");

                            if (exosuit.storageContainer.container.HasRoomFor(pickupable))
                            //if (exosuit.storageContainer.container.HasRoomFor(size.x,size.y))
                            {
                                QModServices.Main.AddCriticalMessage("has room aadding to inv");
                                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "has room aadding to inv");
                                InventoryItem item = new InventoryItem(pickupable);
                                //exosuit.storageContainer.container.UnsafeAdd(item);
                                exosuit.storageContainer.container.AddItem(pickupable);   
                            }
                            else
                            {
                                QModServices.Main.AddCriticalMessage("Has no room");
                                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Has no room");
                                __instance.SpawnResourceFromPrefab(gameObject);
                            }
                        }
                        else
                        {
                            QModServices.Main.AddCriticalMessage("no vehicle found");
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "no vehicle found");
                            __instance.SpawnResourceFromPrefab(gameObject);
                        }
                    }
                    //outcommend just for test. Works as aspected
                    //else if( (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType) && ( (Utils.GetLocalPlayerComp().IsPiloting()) == false ) )
                    //{
                    //    CraftData.AddToInventory(CraftData.GetTechType(gameObject));
                    //}
                    else
                    {
                        QModServices.Main.AddCriticalMessage("Is not piloting and no glove");
                        __instance.SpawnResourceFromPrefab(gameObject);
                    }                   
                    flag = true;
                }
            }
            if (!flag)
            {
                if (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType )
                {
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

        // If somebody ask me why i am doing that instead of getting the Techtype from the Prefab. THIS WAS JUST A TEST FOR AN IDEA FOR SOMETHING ELSE I WAS WORKING ON!
        /*
        private static (GameObject,TechType) Fake_ChooseRandomResource(BreakableResource __instance)
        {
            GameObject result1 = null;
            TechType result2 = TechType.None;
            for (int i = 0; i < __instance.prefabList.Count; i++)
            {
                BreakableResource.RandomPrefab randomPrefab = __instance.prefabList[i];
                PlayerEntropy component = Player.main.gameObject.GetComponent<PlayerEntropy>();
                TechType techType = CraftData.GetTechType(randomPrefab.prefab);
                if (component.CheckChance(techType, randomPrefab.chance))
                {
                    result2 = techType;
                    break;
                }
            }
            return (result1,result2);
        }
        */
    }
}
