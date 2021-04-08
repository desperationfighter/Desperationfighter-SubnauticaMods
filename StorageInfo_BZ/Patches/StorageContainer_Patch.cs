using HarmonyLib;

//transpiller
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace StorageInfo_BZ.Patches
{
    [HarmonyPatch(typeof(StorageContainer))]
    [HarmonyPatch(nameof(StorageContainer.OnHandHover))]
    public static class StorageContainer_OnHandHover_Patch
    {
        #region Prefix
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
        #endregion Prefix

        //---------------------------------------------------------------------------------------------------------------------------------------------------

        #region Transpiler
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var getFullState = typeof(StorageContainer_OnHandHover_Patch).GetMethod("Getfullstate", BindingFlags.Public | BindingFlags.Static);
            var Index = -1;
            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                //if (codes[i].opcode == OpCodes.Ldsfld && codes[i].operand.ToString() == "string [mscorlib]System.String::Empty" && codes[i - 1].opcode == OpCodes.Brtrue_S && codes[i + 2].opcode == OpCodes.Ldstr && (string)codes[i + 2].operand == "Empty")
                if (codes[i].opcode == OpCodes.Ldsfld && codes[i - 1].opcode == OpCodes.Brtrue_S && codes[i + 2].opcode == OpCodes.Ldstr)
                {
                    Index = i;
                    break;
                }
            }

            if (Index > -1)
            {
                codes[Index] = new CodeInstruction(OpCodes.Call, getFullState);
            }
            else
            {

            }

            return codes.AsEnumerable();
        }

        public static string Getfullstate()
        {
            return "MyTest";
        }
        #endregion Transpiler
    }
}
