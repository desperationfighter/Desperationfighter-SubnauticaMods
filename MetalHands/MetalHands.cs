//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//to show a test
using QModManager.API;
//for Logging
using QModManager.Utility;
//For IngameConfig
using SMLHelper.V2.Handlers;
//WorkingSpace
using MetalHands.Managment;
using MetalHands.Items;
//adding Custom Databox
using CustomDataboxes.API;
using System.Collections.Generic;

namespace MetalHands
{
    [QModCore]
    public class MetalHands
    {
        internal static IngameConfigMenu Config { get; private set; }
        internal static TechType GloveBlueprintTechType { get; private set; }
        internal static TechType GloveMK2BlueprintTechType { get; private set; }
        internal static TechType GRAVHANDBlueprintTechType { get; private set; }       
        internal static List<LootDistributionData.BiomeData> BiomesToSpawnIn_pre { get; private set; }
        public static bool IncreasedChunkDrops_exist { get; private set; }

        [QModPatch]
        public static void MetalHands_InitializationMethod()
        {
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();

            var GloveBlueprint = new MetalHands_Blueprint();
            GloveBlueprint.Patch();
            GloveBlueprintTechType = GloveBlueprint.TechType;
            var GloveMK2Blueprint = new MetalHandsMK2();
            GloveMK2Blueprint.Patch();
            GloveMK2BlueprintTechType = GloveMK2Blueprint.TechType;
            var GRAVHANDBlueprint = new Prawn_GravHand();
            GRAVHANDBlueprint.Patch();
            GRAVHANDBlueprintTechType = GRAVHANDBlueprint.TechType;

            if (MetalHands.Config.Config_Hardcore == true)
            {
                BiomesToSpawnIn_pre = new List<LootDistributionData.BiomeData>
                {
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.BloodKelp_TechSite_Scatter,
                        count = 1,
                        probability = 0.05f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.CragField_Grass,
                        count = 1,
                        probability = 0.02f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.UnderwaterIslands_TechSite_Scatter,
                        count = 1,
                        probability = 0.05f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.SeaTreaderPath_TechSite_Scatter,
                        count = 1,
                        probability = 0.10f
                    }
                };
            }
            else
            {
                BiomesToSpawnIn_pre = new List<LootDistributionData.BiomeData>
                {
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.GrassyPlateaus_Grass,
                        count = 1,
                        probability = 0.01f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.SparseReef_Techsite,
                        count = 1,
                        probability = 0.10f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.GrassyPlateaus_TechSite_Scattered,
                        count = 1,
                        probability = 0.05f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.CragField_Grass,
                        count = 1,
                        probability = 0.05f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.BloodKelp_TechSite_Scatter,
                        count = 1,
                        probability = 0.05f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.SeaTreaderPath_TechSite_Scatter,
                        count = 1,
                        probability = 0.1f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.UnderwaterIslands_TechSite_Scatter,
                        count = 1,
                        probability = 0.05f
                    }
                };
            }

            Databox myDatabox = new Databox()
            {
                DataboxID = "MetalHandDatabox",
                PrimaryDescription = "Metal Hand Safety Glove Databox",
                SecondaryDescription = "Contains Crafting Tree for Improved Safety Gloves - Alterrra Copyright",
                BiomesToSpawnIn = BiomesToSpawnIn_pre,
                TechTypeToUnlock = MetalHands.GloveBlueprintTechType
            };
            myDatabox.Patch();

            Logger.Log(Logger.Level.Debug, "MetalHands Initialization");
            Harmony harmony = new Harmony("MetalHands");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "MetalHands Patched");

            IncreasedChunkDrops_exist = QModServices.Main.ModPresent("IncreasedChunkDrops");

            QModServices.Main.AddCriticalMessage("Warning the MetalHands Mod is in BETA Status !");
        }
    }
}
