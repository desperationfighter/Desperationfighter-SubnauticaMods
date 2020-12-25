//default
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace TestMod
{
    [QModCore]
    public static class Desp_TestMod_SN_Class
    {
        [QModPatch]
        public static void Desp_TestMod_SN_InitializationMethod()
        {
            Logger.Log(Logger.Level.Debug, "Desp_TestMod_SN_Initialization");

            Harmony harmony = new Harmony("Desp_TestMod_SN");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.Log(Logger.Level.Info, "Desp_TestMod_SN Patched");

            QModServices.Main.AddCriticalMessage("Warning instable Mod - Don´t use it !");
        }
    }
}
