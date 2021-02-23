using HarmonyLib;
using MetalHands.Managment;
using MetalHands.Items;

using SMLHelper.V2.Utility;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace MetalHands.Patches
{
    /*
    [HarmonyPatch(typeof(Equipment))]
    [HarmonyPatch(nameof(Equipment.GetCount))]
    internal class Equipment_GetCount_Patch
    {
        private static List<TechType> substitutionTargets = new List<TechType>();

        [HarmonyPostfix]
        public static void Postfix(Equipment __instance, ref int __result, TechType techType)
        {
            if ((techType == TechType.ReinforcedGloves) && (Inventory.main.equipment.GetTechTypeInSlot("Gloves") == MetalHands.GloveBlueprintTechType))
            {
                __result++;
            }


            //---------------------------------------------
            //Dictionary<TechType, int> equipCount = __instance.GetInstanceField("equippedCount", BindingFlags.NonPublic | BindingFlags.Instance) as Dictionary<TechType, int>;

            //int c;
            //if(equipCount.TryGetValue(MetalHands.GloveBlueprintTechType, out c))
            //{
            //    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, $"Equipment_GetCount_Patch: found {techType.ToString()} equipped");
            //    __result++;
            //}
        }
    }

    
    [HarmonyPatch(typeof(Player), "EquipmentChanged")]
    internal class Player_EquipmentChanged_Patch
    {
        // Original, unmodified materials.
        private static Material defaultGloveMaterial;
        private static Material defaultSuitMaterial;
        private static Material defaultArmsMaterial;
        private static TechType lastBodyTechType = TechType.None;
        private static TechType lastGlovesTechType = TechType.None;

        // The gloves texture is used for the suit as well, on the arms, so we need to do something about that.
        // The block that generates the glove texture is sizable, so it's made into a function here.
        private static Material GetGloveMaterial(Shader shader, Material OriginalMaterial)
        {
            // if the gloves shader isn't null, add the shader
            if (OriginalMaterial != null)
            {
                Material newMat = new Material(OriginalMaterial);
                // if the suit's shader isn't null, add the shader
                if (shader != null)
                    newMat.shader = shader;
                // add the gloves main Texture when equipped
                newMat.mainTexture = Main.glovesTexture;
                // add  the gloves illum texture when equipped
                newMat.SetTexture(ShaderPropertyID._Illum, Main.glovesIllumTexture);
                // add  the gloves spec texture when equipped
                newMat.SetTexture(ShaderPropertyID._SpecTex, Main.glovesTexture);

                return newMat;
            }

            return null;
        }


        [HarmonyPostfix]
        public static void Postfix(ref Player __instance, string slot, InventoryItem item)
        {
            List<string> mySlots = new List<string>() { "Gloves" };

            bool bUseCustomTex = (Main.suitTexture != null && Main.glovesTexture != null);
            Equipment equipment = Inventory.main.equipment;
            if (equipment == null)
            {
                return;
            }
            if (__instance == null)
            {
                return;
            }
            if (__instance.equipmentModels == null)
            {
                return;
            }

            GameObject playerModel = Player.main.gameObject;
            Shader shader = Shader.Find("MarmosetUBER");
            Renderer reinforcedGloves = playerModel.transform.Find("body/player_view/male_geo/reinforcedSuit/reinforced_suit_01_glove_geo").gameObject.GetComponent<Renderer>();
            if (defaultGloveMaterial == null)
            {
                if (reinforcedGloves != null)
                {
                    // Save a copy of the original material, for use later
                    defaultGloveMaterial = new Material(reinforcedGloves.material);
                }
            }
            Renderer reinforcedSuit = playerModel.transform.Find("body/player_view/male_geo/reinforcedSuit/reinforced_suit_01_body_geo").gameObject.GetComponent<Renderer>();
            if (reinforcedSuit != null)
            {
                if (defaultSuitMaterial == null)
                {
                    // Save a copy of the original material, for use later
                    defaultSuitMaterial = new Material(reinforcedSuit.material);
                }
                if (defaultArmsMaterial == null)
                {
                    // Save a copy of the original material, for use later
                    defaultArmsMaterial = new Material(reinforcedSuit.materials[1]);
                }
            }

            foreach (Player.EquipmentType equipmentType in __instance.equipmentModels)
            {
                bool bChangeTex = false;
                bUseCustomTex = (Main.suitTexture != null && Main.glovesTexture != null);
                string activeSlot = equipmentType.slot;
                TechType techTypeInSlot = equipment.GetTechTypeInSlot(activeSlot);
                if (activeSlot == "Body")
                {
                    bChangeTex = (techTypeInSlot != lastBodyTechType);
                    lastBodyTechType = techTypeInSlot;
                    if (techTypeInSlot == MetalHands.GloveBlueprintTechType)
                    {
                        techTypeInSlot = TechType.ReinforcedDiveSuit;
                    }
                    else
                    {
                        bUseCustomTex = false;
                    }
                }
                else if (activeSlot == "Gloves")
                {
                    bChangeTex = (techTypeInSlot != lastGlovesTechType);
                    lastGlovesTechType = techTypeInSlot;
                    if (techTypeInSlot == Main.prefabGloves.TechType)
                        techTypeInSlot = TechType.ReinforcedGloves;
                    else
                        bUseCustomTex = false;
                }
                //else
                //    continue;

                bool flag = false;
                foreach (Player.EquipmentModel equipmentModel in equipmentType.equipment)
                {
                    //Player.EquipmentModel equipmentModel = equipmentType.equipment[j];
                    bool equipmentVisibility = (equipmentModel.techType == techTypeInSlot);
                    if (bChangeTex)
                    {
                        if (bUseCustomTex)
                        {
                            if (shader == null)
                            {
                                bUseCustomTex = false;
                            }
                            else if (activeSlot == "Gloves" && reinforcedGloves == null)
                            {
                                bUseCustomTex = false;
                            }
                            else if (activeSlot == "Body" && reinforcedSuit == null)
                            {
                                bUseCustomTex = false;
                            }
                        }
                    }

                    flag = (flag || equipmentVisibility);
                    if (equipmentModel.model != null)
                    {
                        if (bChangeTex)
                        {
                            if (bUseCustomTex)
                            {
                                // Apply the Brine Suit texture
                                if (activeSlot == "Gloves")
                                {
                                    // if the gloves shader isn't null, add the shader
                                    if (reinforcedGloves != null // This shouldn't be necessary but I'm taking no chances
                                        && reinforcedGloves.material != null)
                                    {
                                        if (brineGloveMaterial == null)
                                            brineGloveMaterial = GetGloveMaterial(shader, defaultGloveMaterial);

                                        if (brineGloveMaterial != null)
                                            reinforcedGloves.material = brineGloveMaterial;
                                        else
                                        {

                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (activeSlot == "Gloves")
                                {
                                    if (reinforcedGloves != null)
                                    {
                                        if (defaultGloveMaterial != null)
                                            reinforcedGloves.material = defaultGloveMaterial;

                                    }
                                }
                            }
                        }

                        equipmentModel.model.SetActive(equipmentVisibility);
                    }
                }
                if (equipmentType.defaultModel != null)
                {
                    equipmentType.defaultModel.SetActive(!flag);
                }
            }
        }
    }
    */
}