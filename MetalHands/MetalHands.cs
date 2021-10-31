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
    public static class MetalHands
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

            if (MetalHands.Config.Config_Hardcore == true)
            {
                BiomesToSpawnIn_pre = new List<LootDistributionData.BiomeData>
                {
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.BloodKelp_TechSite_Scatter,
                        count = 1,
                        probability = 0.0002f
                    }
                };
            }
            else
            {
                BiomesToSpawnIn_pre = new List<LootDistributionData.BiomeData>
                {
                    new LootDistributionData.BiomeData()
                    {
                        biome = BiomeType.BloodKelp_TechSite_Scatter,
                        count = 1,
                        probability = 0.001f
                    }
                };
            }

            List<SMLHelper.V2.Assets.Spawnable.SpawnLocation> spawnLocations = new List<SMLHelper.V2.Assets.Spawnable.SpawnLocation>();
            //Grand Reef Crash Side - Wreck 10 - external
            spawnLocations.Add(new SMLHelper.V2.Assets.Spawnable.SpawnLocation(new UnityEngine.Vector3(-285.30f, -266.90f, -820.10f)));

            if (MetalHands.Config.Config_Hardcore == false)
            {
                //Seatrader Path - Wreck 7 - external
                spawnLocations.Add(new SMLHelper.V2.Assets.Spawnable.SpawnLocation(new UnityEngine.Vector3(-1141.00f, -193.52f, -776.60f)));
                //MushroomForrest - Escape Pod 24 - external
                spawnLocations.Add(new SMLHelper.V2.Assets.Spawnable.SpawnLocation(new UnityEngine.Vector3(-930.00f, -185.20f, 501.00f)));
                //Mountain - Wreck 19 - internal
                spawnLocations.Add(new SMLHelper.V2.Assets.Spawnable.SpawnLocation(new UnityEngine.Vector3(1068.00f, -281.10f, 1349.20f)));
            }

            Databox myDatabox = new Databox()
            {
                DataboxID = "MetalHandDatabox",
                PrimaryDescription = "Metal Hand Safety Glove",
                SecondaryDescription = "Contains Crafting Blueprint for Improved Safety Gloves - Alterrra Copyright",
                CoordinatedSpawns = spawnLocations,
                BiomesToSpawnIn = BiomesToSpawnIn_pre,
                TechTypeToUnlock = MetalHands.MetalHandsMK1TechType
            };
            myDatabox.Patch();

            Logger.Log(Logger.Level.Debug, "MetalHands Initialization");
            Harmony harmony = new Harmony("MetalHands");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "MetalHands Patched");

            IncreasedChunkDrops_exist = QModServices.Main.ModPresent("IncreasedChunkDrops");
            if(IncreasedChunkDrops_exist)
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
