using HarmonyLib;
using MetalHands.Managment;
using MetalHands.Items;

using UnityEngine;

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
                    if( Utils.GetLocalPlayerComp().GetInMechMode() )
                    {
                        //Vehicle vehicle = Player.main.GetVehicle();
                        Exosuit exosuit = Player.main.currentMountedVehicle as Exosuit;
                        Vector2int size = CraftData.GetItemSize(CraftData.GetTechType(gameObject));

                        Pickupable pickupable = new Pickupable( ??? gameObject);

                        if (exosuit.storageContainer.container.HasRoomFor(pickupable))
                        //if (exosuit.storageContainer.container.HasRoomFor(size.x,size.y))
                        {
                            //exosuit.storageContainer.container.UnsafeAdd(item);
                            exosuit.storageContainer.container.AddItem(pickupable);
                            
                        }
                        else
                        {
                            __instance.SpawnResourceFromPrefab(gameObject);
                        }

                    }
                    else if( Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveMK2BlueprintTechType )
                    {
                        CraftData.AddToInventory(CraftData.GetTechType(gameObject));
                    }
                    else
                    {
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
