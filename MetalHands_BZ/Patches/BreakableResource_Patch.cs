﻿using HarmonyLib;
using MetalHands.Managment;
using MetalHands.Items;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections;
using UWE;
using System.Diagnostics;
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
            HitResource_Patch(__instance);
            return false;
        }

        private static void HitResource_Patch(BreakableResource __instance)
        {
            if (MetalHands_BZ.Config.Config_fastbreak == true | (MetalHands_BZ.Config.Config_ModEnable == true && ( ( Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.GloveBlueprintTechType ) | (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.GloveMK2BlueprintTechType) ) ) )
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
            // Get call stack
            StackTrace stackTrace = new StackTrace();
            // Get calling method name
            string callingmethode = stackTrace.GetFrame(1).GetMethod().Name;

            if(callingmethode == "PunchRock" | callingmethode == "OnImpact")
            {
                return true;
            }
            else
            {
                BreakIntoResources_Patch(__instance);
                return false;
            }
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
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "01-01 - start choose random ress");
                AssetReferenceGameObject assetReferenceGameObject = __instance.ChooseRandomResource();
                if (assetReferenceGameObject != null)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "01-02 - Random Resouce is called");
                    if (Player.main.GetVehicle() is Exosuit exosuit)
                    {
                        var installedmodule = exosuit.modules.GetCount(MetalHands_BZ.GRAVHANDBlueprintTechType);
                        if ( ( (installedmodule > 0) | (MetalHands_BZ.Config.Config_fastcollect == true) ) && exosuit.storageContainer.container.HasRoomFor(1,1) )
                        {
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "01-10 - Start AddToPrawn over randomress");
                            CoroutineHost.StartCoroutine(AddtoPrawn(__instance, exosuit, assetReferenceGameObject));
                        }
                        else
                        {
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "01-11 - Spawn original way randomress");
                            __instance.SpawnResourceFromPrefab(assetReferenceGameObject);
                        }
                    }
                    else
                    {
                        if ((Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.GloveMK2BlueprintTechType) | (MetalHands_BZ.Config.Config_fastcollect == true))
                        {
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "01-20 - Player has glove - randomress");
                            CoroutineHost.StartCoroutine(AddbrokenRestoPlayerInv(__instance,assetReferenceGameObject));
                        }
                        else
                        {
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "03-04 - Spawn original way - defaultress");
                            __instance.SpawnResourceFromPrefab(assetReferenceGameObject);
                        }
                    }
                    flag = true;
                }
            }
            if (!flag)
            {
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "03-01 - default resouce is called");
                if (Player.main.GetVehicle() is Exosuit exosuit)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "03-02 - Start AddToPrawn over defaultress");
                    CoroutineHost.StartCoroutine(AddtoPrawn(__instance, exosuit, __instance.defaultPrefabReference));
                }
                else if ((Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.GloveMK2BlueprintTechType) | (MetalHands_BZ.Config.Config_fastcollect == true))
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "03-03 - Player has glove - defaultress");
                    CoroutineHost.StartCoroutine(AddbrokenRestoPlayerInv(__instance, __instance.defaultPrefabReference));
                }
                else
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "03-04 - Spawn original way - defaultress");
                    __instance.SpawnResourceFromPrefab(__instance.defaultPrefabReference);
                }
            }
            FMODUWE.PlayOneShot(__instance.breakSound, __instance.transform.position, 1f);
            if (__instance.hitFX)
            {
                global::Utils.PlayOneShotPS(__instance.breakFX, __instance.transform.position, Quaternion.Euler(new Vector3(270f, 0f, 0f)), null);
            }
        }

        private static IEnumerator AddtoPrawn(BreakableResource __instance, Exosuit exosuit, AssetReferenceGameObject gameObject)
        {
                CoroutineTask<GameObject> task = AddressablesUtility.InstantiateAsync(gameObject.RuntimeKey as string);
                yield return task;

                GameObject prefab = task.GetResult();
                var pickupable = prefab.GetComponent<Pickupable>();

                pickupable.Initialize();
                var item = new InventoryItem(pickupable);
                exosuit.storageContainer.container.UnsafeAdd(item);

                yield break;
        }

        private static IEnumerator AddbrokenRestoPlayerInv(BreakableResource __instance, AssetReferenceGameObject gameObject)
        {          
            CoroutineTask<GameObject> task = AddressablesUtility.InstantiateAsync(gameObject.RuntimeKey as string);
            yield return task;

            GameObject prefab = task.GetResult();
            var pickupable = prefab.GetComponent<Pickupable>();

            pickupable.Initialize();
            CraftData.AddToInventory(pickupable.GetTechType());

            yield break;
        }
    }
}