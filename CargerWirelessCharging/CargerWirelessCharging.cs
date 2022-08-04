//for Assembly info
using System.Reflection;
//Loading Harmony for Patching
using HarmonyLib;
//Loading QMod as Base
using QModManager.API.ModLoading;
//for Logging
using QModManager.Utility;

namespace CargerWirelessCharging
{
    [QModCore]
    public class CargerWirelessCharging
    {
        [QModPatch]
        public static void CargerWirelessCharging_InitializationMethod()
        {
            Logger.Log(Logger.Level.Debug, "CargerWirelessCharging Initialization");
            Harmony harmony = new Harmony("CargerWirelessCharging");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "CargerWirelessCharging Patched");
        }
    }
}
