using HarmonyLib;

//transpiller
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

//for Logging
using QModManager.Utility;

namespace Snowfoxcloak.Patch
{

    [HarmonyPatch(typeof(IceWormHuntModeTrigger))]
    [HarmonyPatch(nameof(IceWormHuntModeTrigger.OnPlayerEnter))]
    public static class IceWormHuntModeTrigger_OnPlayerEnter_Patch_prefix
    {
        public static bool CloakingModuleinstalled { get; set; }
        public static float ReduceValue { get; set; }

        [HarmonyPrefix]
        public static bool prefix()
        {
            Hoverbike hoverbike = IceWormPhantomManager.ReturnPlayerHoverbike();
            var installedmodule = hoverbike.modules.GetCount(Snowfoxcloak.SnowfoxCloakModuleTechType);
            CloakingModuleinstalled = false;
            if (installedmodule > 0)
            {
                CloakingModuleinstalled = true;
                if(Snowfoxcloak.Config.Config_Fullcloak)
                {
                    ReduceValue = 0f;
                }
                else
                {
                    ReduceValue = 0.05f;
                }
            }
            else if(hoverbike.IceWormReductionModuleActive)
            {
                ReduceValue = 0.3f;
            }
            else
            {
                ReduceValue = 0.3f;
            }
            //yeah yeah i know. that makes no sense but it make sense for fall back i don't know....
            return true;
        }
    }

    [HarmonyPatch(typeof(IceWormHuntModeTrigger))]
    [HarmonyPatch(nameof(IceWormHuntModeTrigger.OnPlayerEnter))]
    public static class IceWormHuntModeTrigger_OnPlayerEnter_Patch_transpiler
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //Deep Logging
            bool deeplogging = true;
            //-----------------------------------------------------------
            if (!deeplogging)
            {
                Logger.Log(Logger.Level.Debug, "Deeploging deactivated");
            }

            Logger.Log(Logger.Level.Debug, "Start Transpiler");

            FieldInfo ReduceVaulue_checked = typeof(IceWormHuntModeTrigger_OnPlayerEnter_Patch_prefix).GetField("ReduceValue");
            bool found = false;
            var Index = -1;
            var codes = new List<CodeInstruction>(instructions);

            //logging before
            if (deeplogging)
            {
                Logger.Log(Logger.Level.Debug, "Deep Logging pre-transpiler:");
                for (int k = 0; k < codes.Count; k++)
                {
                    Logger.Log(Logger.Level.Debug, (string.Format("0x{0:X4}", k) + $" : {codes[k].opcode.ToString()}	{(codes[k].operand != null ? codes[k].operand.ToString() : "")}"));
                }
            }

            //analyse the code to find the right place for injection
            Logger.Log(Logger.Level.Debug, "Start code analyses");
            for (var i = 0; i < codes.Count; i++)
            {
                /*
                	if (hoverbike != null)
	                {
		                num = this.hoverbikeAdditiveHuntValue;
		                if (hoverbike.IceWormReductionModuleActive)
		                {
			                int num2 = Mathf.RoundToInt((float)num * 0.3f);
			                num -= num2;
		                }
	                }
                
                    IL_0031: ldloc.0
                    IL_0032: callvirt  instance bool Hoverbike::get_IceWormReductionModuleActive()
                    IL_0037: brfalse.s IL_004B
                    IL_0039: ldloc.1
                    IL_003A: conv.r4
                    IL_003B: ldc.r4    0.3
                    IL_0040: mul
                    IL_0041: call      int32 [UnityEngine.CoreModule]UnityEngine.Mathf::RoundToInt(float32)
                    IL_0046: stloc.2
                    IL_0047: ldloc.1
                    IL_0048: ldloc.2
                    IL_0049: sub
                    IL_004A: stloc.1
                    IL_004B: ldc.i4.0
                */

                //if (codes[i].opcode == OpCodes.Ldloc_0 && codes[i + 1].opcode == OpCodes.Callvirt && codes[i + 2].opcode == OpCodes.Brfalse_S && codes[i + 5].opcode == OpCodes.Ldc_R4) //for deleting the complete module question
                //if (codes[i].opcode == OpCodes.Ldc_I4_0 && codes[i + 1].opcode == OpCodes.Ldc_I4_S && codes[i + 2].opcode == OpCodes.Call) //for finding the false path on the module check
                
                if(codes[i].opcode == OpCodes.Ldc_R4 && codes[i + 1].opcode == OpCodes.Mul && codes[i + 6].opcode == OpCodes.Sub) //finding the original 0.3f Value in multiplaction for the Reducemodule
                {
                    Logger.Log(Logger.Level.Debug, "Found IL Code Line for Index");
                    Logger.Log(Logger.Level.Debug, $"Index = {Index.ToString()}");
                    found = true;
                    Index = i;
                    break;
                }
            }

            if (found)
            {
                Logger.Log(Logger.Level.Debug, "found true");
            }
            else
            {
                Logger.Log(Logger.Level.Debug, "found false");
            }

            if (Index > -1)
            {
                Logger.Log(Logger.Level.Debug, "Index1 > -1");
                Logger.Log(Logger.Level.Info, "Transpiler injectection position found");
                codes[Index] = new CodeInstruction(OpCodes.Ldsfld, ReduceVaulue_checked);
                //codes.RemoveRange(Index + 1, 14); //for deleting the complete module question
                codes.RemoveRange(Index + 1, 1);
            }
            else
            {
                Logger.Log(Logger.Level.Error, "Index was not found");
            }

            //logging after
            if (deeplogging)
            {
                Logger.Log(Logger.Level.Debug, "Deep Logging after-transpiler:");
                for (int k = 0; k < codes.Count; k++)
                {
                    Logger.Log(Logger.Level.Debug, (string.Format("0x{0:X4}", k) + $" : {codes[k].opcode.ToString()}	{(codes[k].operand != null ? codes[k].operand.ToString() : "")}"));
                }
            }

            Logger.Log(Logger.Level.Debug, "Transpiler end going to return");
            return codes.AsEnumerable();
        }
    }
}
