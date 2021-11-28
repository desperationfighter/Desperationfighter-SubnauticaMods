using HarmonyLib;

//transpiller
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

//for Logging
using QModManager.Utility;

namespace Warnbeforebreak_BZ.Patches
{
    [HarmonyPatch(typeof(BreakableResource))]
    [HarmonyPatch(nameof(BreakableResource.OnHandHover))]
    public static class BreakableResource_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //Deep Logging
            bool deeplogging = true;
            if (!deeplogging)
            {
                Logger.Log(Logger.Level.Debug, "Deeploging deactivated");
            }

            Logger.Log(Logger.Level.Debug, "Start Transpiler");

            var replacefunc = typeof(BreakableResource_Patch).GetMethod("replacefunc", BindingFlags.Public | BindingFlags.Static);
            var stringEmpty = AccessTools.Field(typeof(string), "Empty");
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
[Warnbeforebreak_BZ: DEBUG] 0x0000 : ldsfld HandReticle main
[Warnbeforebreak_BZ: DEBUG] 0x0001 : ldc.i4.0
[Warnbeforebreak_BZ: DEBUG] 0x0002 : ldarg.0
[Warnbeforebreak_BZ: DEBUG] 0x0003 : ldfld System.String breakText
[Warnbeforebreak_BZ: DEBUG] 0x0004 : ldc.i4.1
[Warnbeforebreak_BZ: DEBUG] 0x0005 : ldc.i4.4
[Warnbeforebreak_BZ: DEBUG] 0x0006 : callvirt Void SetText(TextType, System.String, Boolean, Button)
[Warnbeforebreak_BZ: DEBUG] 0x0007 : ldarg.0
[Warnbeforebreak_BZ: DEBUG] 0x0008 : call UnityEngine.GameObject get_gameObject()
[Warnbeforebreak_BZ: DEBUG] 0x0009 : call Boolean CanScan(UnityEngine.GameObject)
[Warnbeforebreak_BZ: DEBUG] 0x000A : ldc.i4.0
[Warnbeforebreak_BZ: DEBUG] 0x000B : ceq
[Warnbeforebreak_BZ: DEBUG] 0x000C : brfalse System.Reflection.Emit.Label
[Warnbeforebreak_BZ: DEBUG] 0x000D : ldsfld HandReticle main
[Warnbeforebreak_BZ: DEBUG] 0x000E : ldc.i4.1
[Warnbeforebreak_BZ: DEBUG] 0x000F : ldarg.0
[Warnbeforebreak_BZ: DEBUG] 0x0010 : ldfld System.String mayContainText
[Warnbeforebreak_BZ: DEBUG] 0x0011 : ldc.i4.1
[Warnbeforebreak_BZ: DEBUG] 0x0012 : ldc.i4.s    45
[Warnbeforebreak_BZ: DEBUG] 0x0013 : callvirt Void SetText(TextType, System.String, Boolean, Button)
[Warnbeforebreak_BZ: DEBUG] 0x0014 : br System.Reflection.Emit.Label
[Warnbeforebreak_BZ: DEBUG] 0x0015 : ldsfld HandReticle main
[Warnbeforebreak_BZ: DEBUG] 0x0016 : ldc.i4.1
[Warnbeforebreak_BZ: DEBUG] 0x0017 : ldsfld System.String Empty <<<<<----------------------------------------------------------------------------   0
[Warnbeforebreak_BZ: DEBUG] 0x0018 : ldc.i4.0                                                                                                       1
[Warnbeforebreak_BZ: DEBUG] 0x0019 : ldc.i4.s    45                                                                                                 2
[Warnbeforebreak_BZ: DEBUG] 0x001A : callvirt Void SetText(TextType, System.String, Boolean, Button)                                                3
[Warnbeforebreak_BZ: DEBUG] 0x001B : ldsfld HandReticle main                                                                                        4
[Warnbeforebreak_BZ: DEBUG] 0x001C : ldc.i4.2                                                                                                       5
[Warnbeforebreak_BZ: DEBUG] 0x001D : ldc.r4  1                                                                                                      6
[Warnbeforebreak_BZ: DEBUG] 0x001E : callvirt Void SetIcon(IconType, Single)                                                                        7
[Warnbeforebreak_BZ: DEBUG] 0x001F : ret                                                                                                            8
*/

                if (codes[i].opcode == OpCodes.Ldsfld && codes[i+1].opcode == OpCodes.Ldc_I4_0 && codes[i+2].opcode == OpCodes.Ldc_I4_S && codes[i + 3].opcode == OpCodes.Callvirt && codes[i + 8].opcode == OpCodes.Ret)
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
                //codes[Index] = new CodeInstruction(OpCodes.ldc_i4_s, replacefunc);
                codes[Index] = new CodeInstruction(OpCodes.Call, replacefunc);
                codes.RemoveRange(Index-1, 1);
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

        public static string replacefunc(BreakableResource __breakableResource)
        {
            return "dummy";
        }

        /*
        // Old Version
        [HarmonyPostfix]
        private static void Postfix(BreakableResource __instance)
        {
            Logger.Log(Logger.Level.Debug, "Breakresource - OnHandover - loaded");
            if ( ! (Player.main.HasInventoryRoom(1, 1) ) )
            {
                Logger.Log(Logger.Level.Debug, "Has NO Room");
                if (!PDAScanner.CanScan(__instance.gameObject))
                {
                    
                }
                else
                {
                    HandReticle.main.SetText(HandReticle.TextType.HandSubscript, "Inventory Full !", false, GameInput.Button.None);
                }
                HandReticle.main.SetIcon(HandReticle.IconType.HandDeny, 1f);
            }
        }
        */
    }
}