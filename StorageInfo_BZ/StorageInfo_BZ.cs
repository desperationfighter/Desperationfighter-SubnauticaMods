//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//for Logging
using QModManager.Utility;

/* 
 * Special Thanks goes to DingoDjango/saipop for the idea from the Base Game of Subnautica.
 * This Version is not a direct port. It is self written but heavy inspired by the original Author.
 */

namespace StorageInfo_BZ
{
    public class StorageInfo_BZ
    {
        [QModPatch]
        public static void StorageInfo_BZ_Initial()
        {
            Logger.Log(Logger.Level.Debug, "StorageInfo_BZ Initialization");

            Harmony harmony = new Harmony("StorageInfo_BZ");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Logger.Log(Logger.Level.Info, "StorageInfo_BZ  Patched");
        }
    }
}
