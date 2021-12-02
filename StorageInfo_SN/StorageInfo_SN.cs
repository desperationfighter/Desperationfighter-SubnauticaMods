//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//for Logging
using QModManager.Utility;

namespace StorageInfo_SN
{
    [QModCore]
    public static class StorageInfo_SN
    {
        [QModPatch]
        public static void StorageInfo_SN_Initial()
        {
            Logger.Log(Logger.Level.Debug, "StorageInfo_SN Initialization");

            Harmony harmony = new Harmony("StorageInfo_SN");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Logger.Log(Logger.Level.Info, "StorageInfo_SN  Patched");
        }
    }
}
