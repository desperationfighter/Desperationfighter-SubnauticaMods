using HarmonyLib;

//transpiller
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

//for Logging
using QModManager.Utility;

namespace StorageInfo_BZ.Patches
{
    [HarmonyPatch(typeof(StorageContainer))]
    [HarmonyPatch(nameof(StorageContainer.OnHandHover))]
    public static class StorageContainer_OnHandHover_Patch
    {
        #region Postfix
        /*
        [HarmonyPostfix]
        public static void Postfix()
        {
            Logger.Log(Logger.Level.Debug, "Postfix");
        }
        */
        #endregion Postfix

        //---------------------------------------------------------------------------------------------------------------------------------------------------

        #region Prefix
        /*
        [HarmonyPrefix]
        public static bool Prefix(StorageContainer __instance)
        {
            //OnHandHover_prefix(__instance);
            return true;
        }

        public static void OnHandHover_prefix(StorageContainer __instance)
        {
            if (!__instance.enabled)
            {
                return;
            }
            if (__instance.disableUseability)
            {
                return;
            }
            Constructable component = __instance.gameObject.GetComponent<Constructable>();
            if (!component || component.constructed)
            {
                HandReticle.main.SetText(HandReticle.TextType.Hand, __instance.hoverText, true, GameInput.Button.LeftHand);

                if (__instance != null)
                {
                    string customInfoText = string.Empty;
                    ItemsContainer container = __instance.container;

                    if (container != null)
                    {
                        if (container.count <= 0)
                        {
                            customInfoText = "Empty";
                        }
                        else if (!container.HasRoomFor(1, 1))
                        {
                            customInfoText = "Full";
                        }
                        else
                        {
                            string Count = container.count.ToString();
                            customInfoText = Count + " Items";
                        }

                        HandReticle.main.SetText(HandReticle.TextType.HandSubscript, customInfoText, true, GameInput.Button.None);
                    }
                }
                else
                {
                    HandReticle.main.SetText(HandReticle.TextType.HandSubscript, __instance.IsEmpty() ? "Empty" : string.Empty, true, GameInput.Button.None);
                }
                HandReticle.main.SetIcon(HandReticle.IconType.Hand, 1f);
            }
        }
        */
        #endregion Prefix

        //---------------------------------------------------------------------------------------------------------------------------------------------------

        #region Transpiler
        
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Logger.Log(Logger.Level.Debug, "Start Transpiler");
            var getFullState = typeof(StorageContainer_OnHandHover_Patch).GetMethod("Getfullstate", BindingFlags.Public | BindingFlags.Static);
            var stringEmpty = AccessTools.Field(typeof(string), "Empty");
            bool found = false;
            var Index = -1;
            var codes = new List<CodeInstruction>(instructions);

            //logging before
            if (true)
            {
                Logger.Log(Logger.Level.Debug, "Deep Logging pre-transpiler:");
                for (int k = 0; k < codes.Count; k++)
                {
                    Logger.Log(Logger.Level.Debug, (string.Format("0x{0:X4}", k) + $" : {codes[k].opcode.ToString()}	{(codes[k].operand != null ? codes[k].operand.ToString() : "")}") );
                }
            }

            for (var i = 0; i < codes.Count; i++)
            {
                //if (codes[i].opcode == OpCodes.Ldsfld && codes[i].operand.ToString() == "string [mscorlib]System.String::Empty" && codes[i - 1].opcode == OpCodes.Brtrue_S && codes[i + 2].opcode == OpCodes.Ldstr && (string)codes[i + 2].operand == "Empty")
                //if (codes[i].opcode == OpCodes.Ldsfld && codes[i - 1].opcode == OpCodes.Brtrue_S && codes[i + 2].opcode == OpCodes.Ldstr)
                //&& codes[i].operand == stringEmpty // that works dedicated
                if (codes[i].opcode == OpCodes.Ldsfld && codes[i + 2].opcode == OpCodes.Ldstr)
                {
                    Logger.Log(Logger.Level.Debug, "Found IL Code Line");
                    Logger.Log(Logger.Level.Debug, "Index =");
                    Logger.Log(Logger.Level.Debug, Index.ToString());
                    Logger.Log(Logger.Level.Debug, "Index > -1");
                    found = true;
                    Index = i;
                    break;
                }
            }

            if(found)
            {
                Logger.Log(Logger.Level.Debug, "found true");
            }
            else
            {
                Logger.Log(Logger.Level.Debug, "found false");
            }

            if (Index > -1)
            {
                Logger.Log(Logger.Level.Debug, "Index > -1");
                codes[Index] = new CodeInstruction(OpCodes.Call, getFullState);

                //int insertindex = Index - 1;
                codes.Insert(Index, new CodeInstruction(OpCodes.Ldarg_0));
            }
            else
            {
                Logger.Log(Logger.Level.Error, "Index was not found");
            }

            //logging after
            if (true)
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

        /*
        public static string Getfullstate()
        {
            Logger.Log(Logger.Level.Debug, "call Getfullstate - TEST");
            return "Mytest";
        }
        */

        
        public static string Getfullstate(StorageContainer _storageContainer)
        {
            Logger.Log(Logger.Level.Debug, "call Getfullstate");
            string fullstate = string.Empty;

            if(!_storageContainer.container.HasRoomFor(1,1))
            {
                Logger.Log(Logger.Level.Debug, "Container is Full - way");
                fullstate = "Full";
            }
            else
            {
                Logger.Log(Logger.Level.Debug, "Container Contains X Item - way");
                Logger.Log(Logger.Level.Debug, "Itemcount =");
                Logger.Log(Logger.Level.Debug, _storageContainer.container.count.ToString());
                fullstate = _storageContainer.container.count.ToString() + " Items";
            }

            return fullstate;
        }
        
        
        #endregion Transpiler
    }
}
