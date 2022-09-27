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
    [HarmonyPatch(typeof(Pickupable))]
    [HarmonyPatch(nameof(Pickupable.OnHandHover))]
    public static class Pickupable_OnHandHover_Patch
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

            Logger.Log(Logger.Level.Debug, "Start Transpiler - Pickupable_OnHandHover_Patch");

            //var replacefunc = typeof(Pickupable_OnHandHover_Patch).GetMethod("replacefunc", BindingFlags.Public | BindingFlags.Static);
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
IL_00F9: ldloc.3        <-------------------------------------------------------------------------------------------------------------- 0
IL_00FA: ldc.i4.0                                                                                                                       1
IL_00FB: ldc.i4.s  44                                                                                                                   2
IL_00FD: callvirt instance void HandReticle::SetText(valuetype HandReticle / TextType, string, bool, valuetype GameInput / Button)      3 
IL_0102: ret                                                                                                                            4
IL_0103: ldarg.0                                                                                                                        5
                
//---------------------

0x004D : callvirt	Void SetText(TextType, System.String, Boolean, Button)      -16
0x004E : ldsfld	HandReticle main                                                -15
0x004F : ldc.i4.1	                                                            -14
0x0050 : ldloc.3	                                                            -13 HandReticle.main.SetText(HandReticle.TextType.HandSubscript, text2, false, GameInput.Button.None);
0x0051 : ldc.i4.0	                                                            -12
0x0052 : ldc.i4.s	44                                                          -11
0x0053 : callvirt	Void SetText(TextType, System.String, Boolean, Button)      -10
0x0054 : ret	                                                                -9
0x0055 : ldsfld	HandReticle main                                                -8
0x0056 : ldc.i4.0	                                                            -7
0x0057 : ldloc.2	                                                            -6
0x0058 : ldc.i4.0	                                                            -5
0x0059 : ldc.i4.4	                                                            -4
0x005A : callvirt	Void SetText(TextType, System.String, Boolean, Button)      -3
0x005B : ldsfld	HandReticle main                                                -2
0x005C : ldc.i4.1	                                                            -1
0x005D : ldloc.3	<---------------------------------------------------------- 0 HandReticle.main.SetText(HandReticle.TextType.HandSubscript, text2, false, GameInput.Button.None);
0x005E : ldc.i4.0	                                                            1
0x005F : ldc.i4.s	44                                                          2
0x0060 : callvirt	Void SetText(TextType, System.String, Boolean, Button)      3
0x0061 : ret	                                                                4
0x0062 : ldarg.0	                                                            5
0x0063 : ldfld	System.Boolean isPickupable                                     6
0x0064 : brfalse	System.Reflection.Emit.Label                                7
0x0065 : ldsfld	Player main                                                     8
0x0066 : ldarg.0	                                                            9
0x0067 : callvirt	Boolean HasInventoryRoom(Pickupable)                        10
0x0068 : brtrue	System.Reflection.Emit.Label                                    11
0x0069 : ldloc.0	                                                            12
0x006A : ldc.i4.0	                                                            13
0x006B : ldloc.1	                                                            14
0x006C : ldc.i4.0	                                                            15
0x006D : call	System.String AsString(TechType, Boolean)                       16
0x006E : ldc.i4.1	                                                            17
0x006F : ldc.i4.s	44                                                          18
0x0070 : callvirt	Void SetText(TextType, System.String, Boolean, Button)      19
0x0071 : ldloc.0	                                                            20
0x0072 : ldc.i4.1	                                                            21
0x0073 : ldstr	InventoryFull                                                   22 main.SetText(HandReticle.TextType.HandSubscript, "InventoryFull", true, GameInput.Button.None);
0x0074 : ldc.i4.1	                                                            23
0x0075 : ldc.i4.s	44                                                          24
0x0076 : callvirt	Void SetText(TextType, System.String, Boolean, Button)      25
0x0077 : ret	                                                                26
0x0078 : ldloc.0	                                                            27
0x0079 : ldc.i4.0	                                                            28
0x007A : ldloc.1	                                                            29
0x007B : ldc.i4.0	                                                            30
0x007C : call	System.String AsString(TechType, Boolean)                       31
0x007D : ldc.i4.1	                                                            32
0x007E : ldc.i4.s	44                                                          33
0x007F : callvirt	Void SetText(TextType, System.String, Boolean, Button)      34
0x0080 : ldloc.0	                                                            35
0x0081 : ldc.i4.1	                                                            36
0x0082 : ldsfld	System.String Empty                                             37 main.SetText(HandReticle.TextType.HandSubscript, string.Empty, false, GameInput.Button.None);
0x0083 : ldc.i4.0	                                                            38
0x0084 : ldc.i4.s	44                                                          39
0x0085 : callvirt	Void SetText(TextType, System.String, Boolean, Button)      40
0x0086 : ret	                                                                41
                      
                 */

                if (codes[i].opcode == OpCodes.Ldloc_3 && codes[i+4].opcode == OpCodes.Ret && codes[i + 5].opcode == OpCodes.Ldarg_0)  //adjust
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

                codes.Insert(Index - 12, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Pickupable_OnHandHover_Patch), nameof(Pickupable_OnHandHover_Patch.GetFreeExoSuitStorage))));
                codes.Insert(Index + 2, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Pickupable_OnHandHover_Patch), nameof(Pickupable_OnHandHover_Patch.GetFreeSpacewithOriginal))));
                codes.Insert(Index + 40, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Pickupable_OnHandHover_Patch), nameof(Pickupable_OnHandHover_Patch.GetFreeSpace))));
                codes.RemoveRange(Index + 39, 1);
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

        public static string GetFreeSpacewithOriginal(string original)
        {
            string returnstring = GetFreeSpace();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(original);
            stringBuilder.AppendLine(returnstring);
            return stringBuilder.ToString();
        }

        public static string GetFreeSpace()
        {
            string returnstring = "";
            if (!(Player.main.HasInventoryRoom(1, 1)))
            {
                HandReticle.main.SetIcon(HandReticle.IconType.HandDeny, 1f);
                returnstring = "Inventory Full !";
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
                returnstring = sizeLeft.ToString() + " of " + Size_xy + " free";
                
            }
            return returnstring;
        }

        public static string GetFreeExoSuitStorage(string original)
        {
            string returnstring = "";

            if (Player.main.GetVehicle() is Exosuit exosuit)
            {
                if(exosuit.storageContainer.container.HasRoomFor(1,1))
                {
                    int Size_x = exosuit.storageContainer.container.sizeX;
                    int Size_y = exosuit.storageContainer.container.sizeY;
                    int Size_xy = Size_y * Size_x;
                    var Items = exosuit.storageContainer.container.GetItemTypes();
                    int usedSize = 0;
                    foreach (var i in Items)
                    {
                        var size = CraftData.GetItemSize(i);
                        int numberofsingletechtype = (exosuit.storageContainer.container.GetItems(i)).Count;
                        Logger.Log(Logger.Level.Debug, $"Techtype = {i.ToString()}");
                        Logger.Log(Logger.Level.Debug, $"Number of items in this Techtype = {numberofsingletechtype.ToString()}");
                        usedSize += size.x * size.y * numberofsingletechtype;
                        Logger.Log(Logger.Level.Debug, $"Used Space of this Techtype = {(size.x * size.y * numberofsingletechtype).ToString()}");

                    }
                    var sizeLeft = Size_xy - usedSize;
                    returnstring = sizeLeft.ToString() + " of " + Size_xy + " free";
                }
                else
                {
                    HandReticle.main.SetIcon(HandReticle.IconType.HandDeny, 1f);
                    returnstring = "Inventory Full !";
                }
            }
            else
            {
                Logger.Log(Logger.Level.Debug, $"Called but not in a Exosuit ?");
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(original);
            stringBuilder.AppendLine(returnstring);
            return stringBuilder.ToString();
        }
    }
}


/*
using HarmonyLib;

namespace Warnbeforebreak.Patches
{
    [HarmonyPatch(typeof(Pickupable))]
    [HarmonyPatch(nameof(Pickupable.OnHandHover))]
    public static class Pickupable_Patch
    {
        [HarmonyPostfix]
        private static void Postfix(Pickupable __instance)
        {
            HandReticle main = HandReticle.main;

                TechType techType = __instance.GetTechType();
                bool flag = __instance.AllowedToPickUp();
                if (flag)
                {
                    string text = string.Empty;
                    string text2 = string.Empty;
                    Exosuit exosuit = Player.main.GetVehicle() as Exosuit;
                    bool flag2 = exosuit == null || exosuit.HasClaw();
                    ProfilingUtils.EndSample(null);
                    if (flag2)
                    {
                        ISecondaryTooltip component = __instance.gameObject.GetComponent<ISecondaryTooltip>();
                        if (component != null)
                        {
                            text2 = component.GetSecondaryTooltip();
                        }
                        text = (__instance.usePackUpIcon ? LanguageCache.GetPackUpText(techType) : LanguageCache.GetPickupText(techType));
                    }
                    if (exosuit)
                    {
                        HandReticle.Hand hand2 = flag2 ? HandReticle.Hand.Left : HandReticle.Hand.None;
                        if (exosuit.leftArmType != TechType.ExosuitClawArmModule)
                        {
                            hand2 = HandReticle.Hand.Right;
                        }
                        main.SetInteractText(text, text2, false, false, hand2);
                    }
                    else
                    {
                        main.SetInteractText(text, text2, false, false, HandReticle.Hand.Left);
                    }
                }
                else if (__instance.isPickupable)
                {
                    main.SetInteractInfo(techType.AsString(false), GetInventoryleftspace());
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
}
*/