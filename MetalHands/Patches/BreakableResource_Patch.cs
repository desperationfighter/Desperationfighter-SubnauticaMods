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

        private static void AddtoPrawn(BreakableResource __instance, Exosuit exosuit, GameObject gameObject)
        {
            var installedmodule = exosuit.modules.GetCount(MetalHands.GRAVHANDBlueprintTechType);
            if ( (installedmodule > 0) | (MetalHands.Config.Config_fastcollect == true) )
            {
                GameObject prefab = GameObject.Instantiate(gameObject);
                var pickupable = prefab.GetComponent<Pickupable>();

                if (exosuit.storageContainer.container.HasRoomFor(pickupable))
                {
                    pickupable = pickupable.Initialize();
                    var item = new InventoryItem(pickupable);

                    exosuit.storageContainer.container.UnsafeAdd(item);
                    return;
                }
            }
            __instance.SpawnResourceFromPrefab(gameObject);
        }

        [HarmonyPostfix]
        private static void Postfix(BreakableResource __instance)
        {
            Collider[] hitColliders = Physics.OverlapSphere(__instance.transform.position, 2f, 1, QueryTriggerInteraction.UseGlobal);
            foreach (Collider hitCollider in hitColliders)
            {
                Pickupable pickupable = hitCollider.gameObject.GetComponentInParent<Pickupable>();
                if (pickupable != null && pickupable.isPickupable)
                {
                    if (Player.main.GetVehicle() is Exosuit exosuit)
                    {
                        QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "93 - Start AddToPrawn with draw");

                        AddtoPrawn(__instance, exosuit, hitCollider.gameObject);
                        GameObject.DestroyImmediate(hitCollider.gameObject);
                    }
                    else if ((Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType) | (MetalHands.Config.Config_fastcollect == true))
                    {
                        QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "94 - Player has glove - draw");

                        CraftData.AddToInventory(CraftData.GetTechType(hitCollider.gameObject));
                        GameObject.DestroyImmediate(hitCollider.gameObject);
                    }
                }
            }
        }
    }
}