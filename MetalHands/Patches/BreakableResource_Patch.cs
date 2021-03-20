using HarmonyLib;
using MetalHands.Managment;
using MetalHands.Items;
using UnityEngine;
//for Logging
using QModManager.API;
using QModManager.Utility;
using System.Collections.Generic;
using System.Collections;

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
    }



    [HarmonyPatch(typeof(BreakableResource))]
    [HarmonyPatch(nameof(BreakableResource.BreakIntoResources))]
    public static class BreakableResource_BreakIntoResources_Patch
    {
        #region Prefix

        [HarmonyPrefix]
        private static bool Prefix(BreakableResource __instance)
        {
            //BreakIntoResources_Patch(__instance);
            //return false;
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
                if (Player.main.GetVehicle() is Exosuit exosuit)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "4 - Start AddToPrawn over defaultress");
                    AddtoPrawn(__instance, exosuit, __instance.defaultPrefab);
                }
                else if( (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType) | (MetalHands.Config.Config_fastcollect == true))
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "7 - Player has glove - defaultress");
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

        private static void AddtoPrawn(BreakableResource __instance, Exosuit exosuit, GameObject gameObject,bool defaultspawn = true)
        {
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "30 - start hitcolider foreach");
            var installedmodule = exosuit.modules.GetCount(MetalHands.GRAVHANDBlueprintTechType);
            if ( (installedmodule > 0) | (MetalHands.Config.Config_fastcollect == true) )
            {
                GameObject prefab = GameObject.Instantiate(gameObject);
                //var pickupable = prefab.GetComponent<Pickupable>();
                var pickupable = prefab.GetComponent<Pickupable>() ?? prefab.GetComponentInParent<Pickupable>();

                if (exosuit.storageContainer.container.HasRoomFor(pickupable) && (pickupable != null && pickupable.isPickupable))
                {
                    pickupable = pickupable.Initialize();
                    var item = new InventoryItem(pickupable);

                    exosuit.storageContainer.container.UnsafeAdd(item);
                    return;
                }
            }
            if (defaultspawn)
            {
                __instance.SpawnResourceFromPrefab(gameObject);
            }
        }

        #endregion Prefix

        [HarmonyPostfix]
        //[HarmonyPriority(600)]
        [HarmonyPriority(Priority.Low)]
        private static void Postfix(BreakableResource __instance)
        {          
          //Collider[] hitColliders = Physics.OverlapSphere(__instance.transform.position, 1f, 1, QueryTriggerInteraction.UseGlobal);
            Collider[] hitColliders = Physics.OverlapSphere(__instance.transform.position, 1f);
            int i = 0;
            foreach (Collider hitCollider in hitColliders)
            {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "70 - start hitcolider foreach");
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, i.ToString());
                    i++;

                Pickupable pickupable = hitCollider.gameObject.GetComponentInParent<Pickupable>();
                
                if(pickupable == null)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "72 - is null");
                }
                if(hitCollider.gameObject.name.Contains("collision"))
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "74 - Name of Gameobject");
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.name);
                }

                if (pickupable != null && pickupable.isPickupable && !hitCollider.gameObject.name.Contains("collision") )
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "82 - is piclkupble");
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "83 - start read status");
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.name);
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.name);
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, hitCollider.gameObject.activeSelf.ToString());
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "84 - finish read status");

                    hitCollider.attachedRigidbody.isKinematic = true; // try stop object for visual indication (why ever true means stop)
                    // hitCollider.gameObject.SetActive(false); //try  disable the object
                    //GameObject.DestroyImmediate(hitCollider.gameObject); //destoy object to reduce invisble objects in the world

                    if (Player.main.GetVehicle() is Exosuit exosuit)
                    {
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "95 - Start AddToPrawn with draw");

                        AddtoPrawn(__instance, exosuit, hitCollider.gameObject,false);
                        
                        //GameObject.DestroyImmediate(hitCollider.gameObject);
                    }
                    else if ((Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType) | (MetalHands.Config.Config_fastcollect == true))
                    {
                            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "98 - Player has glove - draw");

                        CraftData.AddToInventory(CraftData.GetTechType(hitCollider.gameObject));
                        
                        //GameObject.DestroyImmediate(hitCollider.gameObject);
                    }
                }
            }
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