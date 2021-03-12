using HarmonyLib;
using MetalHands.Managment;
using MetalHands.Items;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections;
using UWE;
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
            BreakIntoResources_Patch(__instance);
            return false;

            //---------------
            //bool woho = temptest(__instance);
            //return woho;
        }

        private static bool temptest (BreakableResource __instance)
        {
            if ((Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.GloveMK2BlueprintTechType) | (MetalHands_BZ.Config.Config_fastcollect == true))
            {
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Error, "7 - Player has glove - defaultress");
                CraftData.AddToInventory(TechType.Diamond);
                return false;
            }
            return true;
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
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "51 - start choose random ress");
                AssetReferenceGameObject assetReferenceGameObject = __instance.ChooseRandomResource();
                if (assetReferenceGameObject != null)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "52 - Random Ress not null");
                    //__instance.SpawnResourceFromPrefab(assetReferenceGameObject);
                    //*
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "1 - Random Resouce is called");
                    if (Player.main.GetVehicle() is Exosuit exosuit)
                    {
                        var installedmodule = exosuit.modules.GetCount(MetalHands_BZ.GRAVHANDBlueprintTechType);
                        if ( ( (installedmodule > 0) | (MetalHands_BZ.Config.Config_fastcollect == true) ) && exosuit.storageContainer.container.HasRoomFor(1,1) )
                        {
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "2 - Start AddToPrawn over randomress");
                            CoroutineHost.StartCoroutine(AddtoPrawn(__instance, exosuit, assetReferenceGameObject));
                        }
                        else
                        {
                            __instance.SpawnResourceFromPrefab(assetReferenceGameObject);
                        }
                    }
                    else
                    {
                        if ((Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.GloveMK2BlueprintTechType) | (MetalHands_BZ.Config.Config_fastcollect == true))
                        {
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "6 - Player has glove - randomress");
                            //CraftData.AddToInventory(CraftData.GetTechType(gameObject));
                            //CraftData.AddToInventory(TechType.Lithium);
                            CoroutineHost.StartCoroutine(AddbrokenRestoPlayerInv(__instance,assetReferenceGameObject));
                        }
                        else
                        {
                            __instance.SpawnResourceFromPrefab(assetReferenceGameObject);
                        }
                    }
                    //*/
                    flag = true;
                }
            }
            if (!flag)
            {
                //QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Error, "61 - start spawn default Ress");
                //__instance.SpawnResourceFromPrefab(__instance.defaultPrefabReference);
                //QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Error, "62 - default ress spawned");

                //*
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "3 - default resouce is called");
                if (Player.main.GetVehicle() is Exosuit exosuit)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "4 - Start AddToPrawn over defaultress");
                    CoroutineHost.StartCoroutine(AddtoPrawn(__instance, exosuit, __instance.defaultPrefabReference));
                }
                else if ((Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands_BZ.GloveMK2BlueprintTechType) | (MetalHands_BZ.Config.Config_fastcollect == true))
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "7 - Player has glove - defaultress");
                    //CraftData.AddToInventory(CraftData.GetTechType(__instance.defaultPrefab));
                    //CraftData.AddToInventory(TechType.Diamond);
                    //this.SpawnResourceFromPrefab(this.defaultPrefabReference);
                    CoroutineHost.StartCoroutine(AddbrokenRestoPlayerInv(__instance, __instance.defaultPrefabReference));
                }
                else
                {
                    __instance.SpawnResourceFromPrefab(__instance.defaultPrefabReference);
                }
                //*/
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

        /*
        private static IEnumerator AddtoPrawn(BreakableResource __instance, Exosuit exosuit, AssetReferenceGameObject gameObject)
        {
            var installedmodule = exosuit.modules.GetCount(MetalHands_BZ.GRAVHANDBlueprintTechType);
            if ((installedmodule > 0) | (MetalHands_BZ.Config.Config_fastcollect == true))
            {
                CoroutineTask<GameObject> task = AddressablesUtility.InstantiateAsync(gameObject.RuntimeKey as string);
                yield return task;

                GameObject prefab = task.GetResult();
                var pickupable = prefab.GetComponent<Pickupable>();
                if (exosuit.storageContainer.container.HasRoomFor(pickupable))
                {
                    pickupable.Initialize();
                    var item = new InventoryItem(pickupable);
                    exosuit.storageContainer.container.UnsafeAdd(item);
                }
                else
                {
                    __instance.SpawnResourceFromPrefab(gameObject);
                }
                yield break;
            }
            else
            {
                __instance.SpawnResourceFromPrefab(gameObject);
            }
        }
        */

        /*
        private static IEnumerator AddtoPrawn(BreakableResource __instance, Exosuit exosuit, AssetReferenceGameObject assetReferenceGameObject)
        {
            var installedmodule = exosuit.modules.GetCount(MetalHands.GRAVHANDBlueprintTechType);
            if ( (installedmodule > 0) | (MetalHands.Config.Config_fastcollect == true) )
            {
                //AssetReferenceGameObject prefab = GameObject.Instantiate(assetReferenceGameObject);
                //var pickupable = prefab.GetComponent<Pickupable>();

                //TaskResult<AssetReferenceGameObject> result = new TaskResult<AssetReferenceGameObject>();
                yield return assetReferenceGameObject.InstantiateAsync(result);
                assetReferenceGameObject = result.Get();

                if (exosuit.storageContainer.container.HasRoomFor(pickupable))
                {
                    pickupable = pickupable.Initialize();
                    var item = new InventoryItem(pickupable);

                    exosuit.storageContainer.container.UnsafeAdd(item);
                    return;
                }
            }
            __instance.SpawnResourceFromPrefab(assetReferenceGameObject);
        }
        */
    }
}