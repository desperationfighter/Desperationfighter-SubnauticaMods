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

/// <summary>
/// WARNING THIS IS A COPY OF "RandyKnapp"´s MOD "LongLockerNames"
/// https://github.com/RandyKnapp/SubnauticaModSystem/tree/master/SubnauticaModSystem
/// IT IS ONLY FOR DEBUG PURPOS AND SHOULD NOT BE USED. ALL CHANGES WILL GO TO THE ORIGINAL MOD AFTER FINISHED
/// THE ONLY REASON WHY I BUILD IT THIS WAY IS: BUILDING IT FROM SCRATCH IS EASIER AS CHANING THE ORIGINAL BECAUSE THERE ARE SOME MAJOR CHANGES.
/// WARNING DON`T USE THIS MOD
/// </summary>

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
