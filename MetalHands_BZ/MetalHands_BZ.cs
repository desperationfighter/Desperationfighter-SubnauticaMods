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
    public static class MetalHands_BZ
    {
        internal static IngameConfigMenu Config { get; private set; }
        internal static TechType MetalHandsMK1TechType { get; private set; }
        internal static TechType MetalHandsMK2TechType { get; private set; }
        internal static TechType MetalHandsClawModuleTechType { get; private set; }
        internal static List<LootDistributionData.BiomeData> BiomesToSpawnIn_pre { get; private set; }
        public static bool IncreasedChunkDrops_exist { get; private set; }

        [QModPatch]
        public static void MetalHands_InitializationMethod()
        {
            Config = OptionsPanelHandler.Main.RegisterModOptions<IngameConfigMenu>();

            var GloveBlueprint = new MetalHandsMK1();
            GloveBlueprint.Patch();
            MetalHandsMK1TechType = GloveBlueprint.TechType;
            var GloveMK2Blueprint = new MetalHandsMK2();
            GloveMK2Blueprint.Patch();
            MetalHandsMK2TechType = GloveMK2Blueprint.TechType;
            var GRAVHANDBlueprint = new MetalHandsClawModule();
            GRAVHANDBlueprint.Patch();
            MetalHandsClawModuleTechType = GRAVHANDBlueprint.TechType;

            if (MetalHands_BZ.Config.Config_Hardcore == true)
            {
                BiomesToSpawnIn_pre = new List<LootDistributionData.BiomeData>
                {
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.TreeSpires_Ground,
                        count = 1,
                        probability = 0.01f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.ArcticKelp_CaveInner_Sand,
                        count = 1,
                        probability = 0.01f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.GlacialBasin_BikeCrashSite,
                        count = 1,
                        probability = 0.01f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.TwistyBridges_Cave_Ground,
                        count = 1,
                        probability = 0.01f
                    }
                };
            }
            else
            {
                BiomesToSpawnIn_pre = new List<LootDistributionData.BiomeData>
                {
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.TreeSpires_Ground,
                        count = 1,
                        probability = 0.05f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.ArcticKelp_CaveInner_Sand,
                        count = 1,
                        probability = 0.02f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.GlacialBasin_BikeCrashSite,
                        count = 1,
                        probability = 0.01f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.TwistyBridges_Cave_Ground,
                        count = 1,
                        probability = 0.01f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.TwistyBridges_Shallow_Coral,
                        count = 1,
                        probability = 0.01f
                    },
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.LilyPads_Deep_Ground,
                        count = 1,
                        probability = 0.01f
                    },
                };
            }

            Databox myDatabox = new Databox()
            {
                DataboxID = "MetalHandDatabox",
                PrimaryDescription = "Metal Hand Safety Glove Databox",
                SecondaryDescription = "Contains Crafting Tree for Improved Safety Gloves - Alterrra Copyright",
                BiomesToSpawnIn = BiomesToSpawnIn_pre,
                TechTypeToUnlock = MetalHands_BZ.MetalHandsMK1TechType
            };
            myDatabox.Patch();

            Logger.Log(Logger.Level.Debug, "MetalHands_BZ Initialization");
            Harmony harmony = new Harmony("MetalHands_BZ");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "MetalHands_BZ Patched");

            IncreasedChunkDrops_exist = QModServices.Main.ModPresent("IncreasedChunkDrops");
            if (IncreasedChunkDrops_exist)
            {
                Logger.Log(Logger.Level.Info, "MetalHands has detected Increased Chunk Drops");
                ErrorMessage.AddMessage("Attention MetalHands does not work properly with Increased Chunk Drops");
                //QModServices.Main.AddCriticalMessage("Attention MetalHands does not work properly with Increased Chunk Drops");
            }
            else
            {
                Logger.Log(Logger.Level.Info, "MetalHands has NOT detected Increased Chunk Drops");
            }

            //QModServices.Main.AddCriticalMessage("Warning the MetalHands Mod is in BETA Status !");
        }
    }
}
