using HarmonyLib;

//transpiller
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

//for Logging
using QModManager.Utility;

namespace Warnbeforebreak.Patches
{
    /*
    [HarmonyPatch(typeof(BreakableResource))]
    [HarmonyPatch(nameof(BreakableResource.OnHandHover))]
    public static class BreakableResource_Patch
    {
        [HarmonyPostfix]
        private static void Postfix(BreakableResource __instance)
        {
            HandReticle.main.SetInteractText(__instance.breakText, GetInventoryleftspace());
            if (!(Player.main.HasInventoryRoom(1, 1)))
            {
                HandReticle.main.SetIcon(HandReticle.IconType.HandDeny, 1f);
            }
        }

        public static string GetInventoryleftspace()
        {
            if (!(Player.main.HasInventoryRoom(1, 1)))
            {
                return "Inventory Full !";
            }
            else
            {
                Inventory Inv = Player.main.GetComponent<Inventory>();
                int Size_x = Inv.container.sizeX;
                int Size_y = Inv.container.sizeY;
                int Size_xy = Size_y * Size_x;
                var Items = Inv.container.GetItemTypes();
                int usedSize = 0;
                foreach (var i in Items)
                {
                    var size = CraftData.GetItemSize(i);
                    int numberofsingletechtype = (Inv.container.GetItems(i)).Count;
                    usedSize += size.x * size.y * numberofsingletechtype;
                }
                var sizeLeft = Size_xy - usedSize;
                string returnstring = sizeLeft.ToString() + " of " + Size_xy + " free";
                return returnstring;
            }

        }
    }
    */

    [HarmonyPatch(typeof(BreakableResource))]
    [HarmonyPatch(nameof(BreakableResource.OnHandHover))]
    public static class BreakableResource_Patch
    {
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //Deep Logging
            bool deeplogging = false;
            if (!deeplogging)
            {
                Logger.Log(Logger.Level.Debug, "Deeploging deactivated");
            }

            Logger.Log(Logger.Level.Debug, "Start Transpiler - BreakableResource_Patch");

            var replacefunc = typeof(BreakableResource_Patch).GetMethod("replacefunc", BindingFlags.Public | BindingFlags.Static);
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
IL_0000: ldsfld class HandReticle HandReticle::main
IL_0005: ldc.i4.0
IL_0006: ldarg.0
IL_0007: ldfld string BreakableResource::breakText
IL_000C: ldc.i4.1
IL_000D: ldc.i4.4
IL_000E: callvirt instance void HandReticle::SetText(valuetype HandReticle/TextType, string, bool, valuetype GameInput/Button)
IL_0013: ldsfld class HandReticle HandReticle::main
IL_0018: ldc.i4.1
IL_0019: ldsfld string[mscorlib] System.String::Empty <<<<<----------------------------------------------------------------------------     0
IL_001E: ldc.i4.0                                                                                                                           1
IL_001F: ldc.i4.s  44                                                                                                                       2
IL_0021: callvirt instance void HandReticle::SetText(valuetype HandReticle/TextType, string, bool, valuetype GameInput/Button)              3
IL_0026: ldsfld class HandReticle HandReticle::main                                                                                         4
IL_002B: ldc.i4.2                                                                                                                           5
IL_002C: ldc.r4    1                                                                                                                        6
IL_0031: callvirt instance void HandReticle::SetIcon(valuetype HandReticle/IconType, float32)                                               7
IL_0036: ret                                                                                                                                8
*/
                if (codes[i].opcode == OpCodes.Ldsfld && codes[i + 1].opcode == OpCodes.Ldc_I4_0 && codes[i + 2].opcode == OpCodes.Ldc_I4_S && codes[i + 3].opcode == OpCodes.Callvirt && codes[i + 8].opcode == OpCodes.Ret)
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
                codes[Index] = new CodeInstruction(OpCodes.Call, replacefunc);
                codes.RemoveRange(Index, 0);
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

        public static string replacefunc()
        {
            if (!(Player.main.HasInventoryRoom(1, 1)))
            {
                return "Inventory Full !";
            }
            else
            {
                Inventory Inv = Player.main.GetComponent<Inventory>();
                int Size_x = Inv.container.sizeX;
                int Size_y = Inv.container.sizeY;
                int Size_xy = Size_y * Size_x;
                var Items = Inv.container.GetItemTypes();
                int usedSize = 0;
                foreach (var i in Items)
                {
                    var size = CraftData.GetItemSize(i);
                    int numberofsingletechtype = (Inv.container.GetItems(i)).Count;
                    Logger.Log(Logger.Level.Debug, $"Techtype = {i.ToString()}");
                    Logger.Log(Logger.Level.Debug, $"Number of items in this Techtype = {numberofsingletechtype.ToString()}");
                    usedSize += size.x * size.y * numberofsingletechtype;
                    Logger.Log(Logger.Level.Debug, $"Used Space of this Techtype = {(size.x * size.y * numberofsingletechtype).ToString()}");

                }
                var sizeLeft = Size_xy - usedSize;
                string returnstring = sizeLeft.ToString() + " of " + Size_xy + " free";
                return returnstring;
            }

        }

        [HarmonyPostfix]
        private static void Postfix(BreakableResource __instance)
        {
            Logger.Log(Logger.Level.Debug, "Breakresource - OnHandover - loaded");
            if (!(Player.main.HasInventoryRoom(1, 1)))
            {
                Logger.Log(Logger.Level.Debug, "Has NO Room");
                HandReticle.main.SetIcon(HandReticle.IconType.HandDeny, 1f);
            }
        }
    }
}