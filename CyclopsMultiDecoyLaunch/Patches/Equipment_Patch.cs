using HarmonyLib;
//for Logging
using QModManager.Utility;

namespace CyclopsMultiDecoyLaunch.Patches
{
    /*
    [HarmonyPatch(typeof(Equipment))]
    [HarmonyPatch(nameof(Equipment.AddSlot))]
    public static class Equipment_AddSlot_Patch
    {
        [HarmonyPrefix]
        private static bool Prefix(Equipment __instance)
        {
            Equipment.slotMapping.Add("Decoy6",EquipmentType.DecoySlot);
            return true;
        }
    }
    */

    [HarmonyPatch(typeof(Equipment))]
    public static class Equipment_Patch
    {
        
        /*[HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            EquipmentType et = EquipmentType.DecoySlot;

            //Deep Logging
            bool deeplogging = true;
            if (!deeplogging)
            {
                Logger.Log(Logger.Level.Debug, "Deeploging deactivated");
            }

            Logger.Log(Logger.Level.Debug, "Start Transpiler - Equipment_Patch");
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
                //if (codes[i].opcode == OpCodes.Ldloc_3 && codes[i + 4].opcode == OpCodes.Ret && codes[i + 5].opcode == OpCodes.Ldarg_0)
                if (codes[i].opcode == OpCodes.Ldstr && codes[i].operand == "DecoySlot5" && codes[i-1].opcode == OpCodes.Dup && codes[i + 1].opcode == OpCodes.Ldc_I4_S)
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

                //codes.Insert(Index - 12, new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Pickupable_OnHandHover_Patch), nameof(Pickupable_OnHandHover_Patch.GetFreeExoSuitStorage))));
                //codes.RemoveRange(Index + 39, 1);
                codes.Insert(Index + 2, new CodeInstruction(OpCodes.Dup));
                codes.Insert(Index + 3, new CodeInstruction(OpCodes.Ldstr, "DecoySlot6"));
                codes.Insert(Index + 4, new CodeInstruction(OpCodes.Ldc_I4_S,16));
                codes.Insert(Index + 5, new CodeInstruction(OpCodes.Callvirt, et));
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
        */
    }
}
