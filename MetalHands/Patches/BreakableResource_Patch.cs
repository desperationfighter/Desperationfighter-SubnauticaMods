using HarmonyLib;
using MetalHands.Managment;
using MetalHands.Items;
using UnityEngine;
//for Logging
using QModManager.API;
using QModManager.Utility;
using System.Collections.Generic;
using System.Collections;
using UWE;

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
            //HitResource_Patch_Prefix(__instance);
            //return false;
            return true;
        }

        private static void HitResource_Patch_Prefix(BreakableResource __instance)
        {
            if (MetalHands.Config.Config_fastbreak == true | ( MetalHands.Config.Config_ModEnable == true && ( ( Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveBlueprintTechType ) | (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType) ) ) )
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

        [HarmonyPostfix]
        private static void Postfix(BreakableResource __instance)
        {
            HitResource_Patch_Postfix(__instance);
        }

        private static void HitResource_Patch_Postfix(BreakableResource __instance)
        {
            if(__instance.hitsToBreak > 0)
            {
                if (MetalHands.Config.Config_fastbreak == true | (MetalHands.Config.Config_ModEnable == true && ((Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveBlueprintTechType) | (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType))))
                {
                    __instance.hitsToBreak = 0;
                    __instance.BreakIntoResources();
                }
            }
        }
    }



    [HarmonyPatch(typeof(BreakableResource))]
    [HarmonyPatch(nameof(BreakableResource.BreakIntoResources))]
    public static class BreakableResource_BreakIntoResources_Patch
    {
        #region BreakIntoResources-Prefix

        [HarmonyPrefix]
        private static bool Prefix(BreakableResource __instance)
        {
            if( !(MetalHands.Config.Config_forceprefix_opverridepostfix) & (MetalHands.IncreasedChunkDrops_exist | MetalHands.Config.Config_forcepostfix))
            {
                //run original
                return true;
            }
            else
            {
                //run custom
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
                GameObject gameObject = __instance.ChooseRandomResource();
                if (gameObject)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "1 - Random Resouce is called");
                    if (Player.main.GetVehicle() is Exosuit exosuit)
                    {
                        QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "2 - Start AddToPrawn over randomress");
                        AddtoPrawn(__instance, exosuit, gameObject);
                    }
                    else
                    {
                        if ( (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType) | (MetalHands.Config.Config_fastcollect == true))
                        {
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "6 - Player has glove - randomress");
                            Vector2int size = CraftData.GetItemSize(CraftData.GetTechType(gameObject));
                            Inventory inventory = Inventory.Get();
                            if (inventory.HasRoomFor(size.x, size.y))
                            {
                                CraftData.AddToInventory(CraftData.GetTechType(gameObject));
                            }
                            else
                            {
                                __instance.SpawnResourceFromPrefab(gameObject);
                            }
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
                if (Player.main.GetVehicle() is Exosuit exosuit)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "4 - Start AddToPrawn over defaultress");
                    AddtoPrawn(__instance, exosuit, __instance.defaultPrefab);
                }
                else if( (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType) | (MetalHands.Config.Config_fastcollect == true))
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "7 - Player has glove - defaultress");
                    Vector2int size = CraftData.GetItemSize(CraftData.GetTechType(__instance.defaultPrefab));
                    Inventory inventory = Inventory.Get();
                    if (inventory.HasRoomFor(size.x,size.y))
                    {
                        CraftData.AddToInventory(CraftData.GetTechType(__instance.defaultPrefab));
                    }
                    else
                    {
                        __instance.SpawnResourceFromPrefab(__instance.defaultPrefab);
                    }
                    
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

        private static bool AddtoPrawn(BreakableResource __instance, Exosuit exosuit, GameObject gameObject,bool defaultspawn = true, bool skipinstantinate = false)
        {
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "30 - start hitcolider foreach");
            var installedmodule = exosuit.modules.GetCount(MetalHands.GRAVHANDBlueprintTechType);
            if ( (installedmodule > 0) | (MetalHands.Config.Config_fastcollect == true) )
            {
                GameObject prefab;
                if (skipinstantinate)
                {
                    prefab = gameObject;
                }
                else
                {
                    prefab = GameObject.Instantiate(gameObject);
                } 
                //var pickupable = prefab.GetComponent<Pickupable>();
                var pickupable = prefab.GetComponent<Pickupable>() ?? prefab.GetComponentInParent<Pickupable>();

                if (exosuit.storageContainer.container.HasRoomFor(pickupable) && (pickupable != null && pickupable.isPickupable))
                {
                    if (skipinstantinate)
                    {                        
                        gameObject.SetActive(false);
                        GameObject.Destroy(gameObject);
                    }
                    else
                    {
                        pickupable = pickupable.Initialize();
                    }
                    var item = new InventoryItem(pickupable);

                    exosuit.storageContainer.container.UnsafeAdd(item);
                    return true;
                }
            }
            if (defaultspawn)
            {
                __instance.SpawnResourceFromPrefab(gameObject);
            }
            return false;
        }

        #endregion BreakIntoResources-Prefix


        /*
        [HarmonyPostfix]
        //[HarmonyPriority(600)]
        [HarmonyPriority(Priority.Low)]
        private static void Postfix(BreakableResource __instance)
        {          
          //Collider[] hitColliders = Physics.OverlapSphere(__instance.transform.position, 1f, 1, QueryTriggerInteraction.UseGlobal);
            Collider[] hitColliders = Physics.OverlapSphere(__instance.transform.position, 1.5f);
            int i = 0;
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "60 - start hitcolider foreach");
            foreach (Collider hitCollider in hitColliders)
            {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "61 - start single hitcolider from foreach");
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, i.ToString());
                    i++;

                Pickupable pickupable = hitCollider.gameObject.GetComponentInParent<Pickupable>();
                
                if(pickupable == null)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "62 - is null");
                }
                if(hitCollider.gameObject.name.Contains("collision"))
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "64 - Name of Gameobject in if collision statement");
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.name);
                }
                else if(hitCollider.gameObject.name.Contains("clone"))
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "66 - Name of Gameobject in if clone statement");
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.name);
                }

                if (pickupable != null && pickupable.isPickupable)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "72 - is pickupable");
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "73 - start read status");
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.name);
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.name);
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.activeSelf.ToString());
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.attachedRigidbody.collisionDetectionMode.ToString());
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "74 - finish read status");

#if null
                    try
                    {
                        hitCollider.attachedRigidbody.isKinematic = true;
                        QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "77 - try");
                    }
                    catch
                    {
                        QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "76 - Catch");
                    }
#endif

                    //hitCollider.attachedRigidbody.isKinematic = true; // try stop object for visual indication (why ever true means stop)
                    // hitCollider.gameObject.SetActive(false); //try  disable the object
                    //GameObject.DestroyImmediate(hitCollider.gameObject); //destoy object to reduce invisble objects in the world

                    if (Player.main.GetVehicle() is Exosuit exosuit)
                    {
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "85 - Start AddToPrawn with draw");

                        AddtoPrawn(__instance, exosuit, hitCollider.gameObject,false);
                        
                        //GameObject.DestroyImmediate(hitCollider.gameObject);
                    }
                    else if ((Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType) | (MetalHands.Config.Config_fastcollect == true))
                    {
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "88 - Player has glove - draw");

                        CraftData.AddToInventory(CraftData.GetTechType(hitCollider.gameObject));
                        
                        //GameObject.DestroyImmediate(hitCollider.gameObject);
                    }

                    
                }
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "90 - stop single hitcolider from foreach");
            }
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "91 - stop foreach");
        }
        */

        [HarmonyPostfix]
        [HarmonyPriority(Priority.Low)]
        private static void Postfix(BreakableResource __instance)
        {
            if ( !(MetalHands.Config.Config_forceprefix_opverridepostfix) & (MetalHands.IncreasedChunkDrops_exist | MetalHands.Config.Config_forcepostfix) )
            {
                //run custom
                CoroutineHost.StartCoroutine(ProcessHitCollider(__instance));
            }
            else
            {
                //do nothing or run original only
            }
        }

        private static IEnumerator ProcessHitCollider(BreakableResource __instance)
        {
            yield return new WaitForSeconds(0.3f);

            Collider[] hitColliders = Physics.OverlapSphere(__instance.transform.position, 2f);
            int i = 0;
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "60 - start hitcolider foreach");
            foreach (Collider hitCollider in hitColliders)
            {
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "61 - start single hitcolider from foreach");
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, i.ToString());
                i++;

                Pickupable pickupable = hitCollider.gameObject.GetComponentInParent<Pickupable>();

                if (pickupable == null)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "62 - is null");
                }
                if (hitCollider.gameObject.name.Contains("collision"))
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "64 - Name of Gameobject in if collision statement");
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.name);
                }
                else if (hitCollider.gameObject.name.Contains("clone"))
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "66 - Name of Gameobject in if clone statement");
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.name);
                }

                if (pickupable != null && pickupable.isPickupable)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "72 - is pickupable");
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "73 - start read status");
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.name);
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.name);
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.activeSelf.ToString());
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.attachedRigidbody.collisionDetectionMode.ToString());
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.layer.ToString());
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "74 - finish read status");

                    if (Player.main.GetVehicle() is Exosuit exosuit)
                    {
                        QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "81 - Start AddToPrawn with draw");

                        //AddtoPrawn(__instance, exosuit, hitCollider.gameObject, false,false);
                        #region internal PRAWN
                        var installedmodule = exosuit.modules.GetCount(MetalHands.GRAVHANDBlueprintTechType);
                        QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "82 - Installed PRAWN Module");
                        QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, installedmodule.ToString());
                        if ((installedmodule > 0) | (MetalHands.Config.Config_fastcollect == true))
                        {
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "83 - PRAWN Allowed to Quickpick");
                            if (exosuit.storageContainer.container.HasRoomFor(pickupable) )
                            {
                                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "84 - PRAWN has Room for Quickpick");
                                var item = new InventoryItem(pickupable);
                                exosuit.storageContainer.container.UnsafeAdd(item);
                                hitCollider.gameObject.SetActive(false);
                                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "85 - after PRAWN quickpick disable gameobject");
                                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.activeSelf.ToString());
                                //GameObject.DestroyImmediate(hitCollider.gameObject);
                                GameObject.Destroy(hitCollider.gameObject);
                                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "86 - after PRAWN Gameobject destroyed");
                            }
                        }
                        #endregion internal PRAWN
                    }
                    else if ((Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType) | (MetalHands.Config.Config_fastcollect == true))
                    {
                        QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "88 - Player has glove - draw");

                        Inventory inventory = Inventory.Get();
                        if(inventory.HasRoomFor(pickupable))
                        {
                            CraftData.AddToInventory(CraftData.GetTechType(hitCollider.gameObject), spawnIfCantAdd: false);

                            hitCollider.gameObject.SetActive(false);
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "89 - Status of Gameobject after setting false");
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.activeSelf.ToString());
                            //GameObject.DestroyImmediate(hitCollider.gameObject);
                            GameObject.Destroy(hitCollider.gameObject);
                        }

                    }

                }
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "90 - stop single hitcolider from foreach");
            }
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "91 - stop foreach");
        }

        private static IEnumerator ExampleCoroutine()
        {
            //Print the time of when the function is first called.
            Debug.Log("Started Coroutine at timestamp : " + Time.time);

            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(3);

            //After we have waited 5 seconds print the time again.
            Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        }
    }
}