using HarmonyLib;

//transpiller
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

//for Logging
using QModManager.Utility;

namespace StorageInfo_BZ.Patches
{
    [HarmonyPatch(typeof(StorageContainer))]
    [HarmonyPatch(nameof(StorageContainer.OnHandHover))]
    public static class StorageContainer_OnHandHover_Patch
    { 
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //Deep Logging
            bool deeplogging = false;
            if(!deeplogging)
            {
                Logger.Log(Logger.Level.Debug, "Deeploging deactivated");
            }
            
            Logger.Log(Logger.Level.Debug, "Start Transpiler");

            var getFullState = typeof(StorageContainer_OnHandHover_Patch).GetMethod("Getfullstate", BindingFlags.Public | BindingFlags.Static);
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
                    Logger.Log(Logger.Level.Debug, (string.Format("0x{0:X4}", k) + $" : {codes[k].opcode.ToString()}	{(codes[k].operand != null ? codes[k].operand.ToString() : "")}") );
                }
            }

            //analyse the code to find the right place for injection
            Logger.Log(Logger.Level.Debug, "Start code analyses");
            for (var i = 0; i < codes.Count; i++)
            {
                /*
                1 IL_0048: call instance bool StorageContainer::IsEmpty()
                2 IL_004D: brtrue.s IL_0056
                3 IL_004F: ldsfld    string[mscorlib] System.String::Empty
                4 IL_0054: br.s IL_005B
                5 IL_0056: ldstr     "Empty"

                [StorageInfo_BZ:DEBUG] 0x001C : call	Boolean IsEmpty()
                [StorageInfo_BZ:DEBUG] 0x001D : brtrue	System.Reflection.Emit.Label
                [StorageInfo_BZ:DEBUG] 0x001E : ldsfld	System.String Empty
                [StorageInfo_BZ:DEBUG] 0x001F : br	System.Reflection.Emit.Label
                [StorageInfo_BZ:DEBUG] 0x0020 : ldstr	Empty
                */

                if (codes[i].opcode == OpCodes.Call && codes[i+2].opcode == OpCodes.Ldsfld && codes[i + 4].opcode == OpCodes.Ldstr)
                    //codes[i].opcode == OpCodes.Call && codes[i + 1].opcode == OpCodes.Brtrue && codes[i + 2].opcode == OpCodes.Ldsfld && codes[i + 3].opcode == OpCodes.Br && codes[i + 4].opcode == OpCodes.Ldstr && codes[i + 4].operand == stringEmpty
                    //codes[i].opcode == OpCodes.Ldsfld && codes[i + 2].opcode == OpCodes.Ldstr && codes[i].operand == stringEmpty
                {
                    Logger.Log(Logger.Level.Debug, "Found IL Code Line for Index");
                    Logger.Log(Logger.Level.Debug, $"Index = {Index.ToString()}");
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
                Logger.Log(Logger.Level.Debug, "Index1 > -1");
                Logger.Log(Logger.Level.Info, "Transpiler injectection position found");
                codes[Index] = new CodeInstruction(OpCodes.Call, getFullState);
                codes.RemoveRange(Index + 1, 4);
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

        public static string Getfullstate(StorageContainer _storageContainer)
        {
            Logger.Log(Logger.Level.Debug, "call Getfullstate");

            var items = _storageContainer.container.GetItemTypes();
            int itemscount = _storageContainer.container.count;
            Logger.Log(Logger.Level.Debug, $"Itemcount all Items = {itemscount.ToString()}");
            int origSize = _storageContainer.container.sizeX * _storageContainer.container.sizeY;
            Logger.Log(Logger.Level.Debug, $"Original Max Size = {origSize.ToString()}");
            int usedSize = 0;
            foreach (var i in items)
            {
                var size = TechData.GetItemSize(i);
                int numberofsingletechtype = (_storageContainer.container.GetItems(i)).Count;
                Logger.Log(Logger.Level.Debug, $"Techtype = {i.ToString()}");
                Logger.Log(Logger.Level.Debug, $"Number of items in this Techtype = {numberofsingletechtype.ToString()}");
                usedSize += size.x * size.y * numberofsingletechtype;
                Logger.Log(Logger.Level.Debug, $"Used Space of this Techtype = {(size.x * size.y * numberofsingletechtype).ToString()}");
            }
            var sizeLeft = origSize - usedSize;
            Logger.Log(Logger.Level.Debug, $"Used Space off all Techtypes = {usedSize.ToString()}");

            StringBuilder stringBuilder = new StringBuilder();
            if (!_storageContainer.container.HasRoomFor(1,1))
            {
                Logger.Log(Logger.Level.Debug, "Container is Full - way");
                stringBuilder.AppendLine("Full - " + itemscount + " Items stored");
                stringBuilder.AppendLine($"{sizeLeft} of {origSize} free");
            }
            else if(_storageContainer.IsEmpty())
            {
                Logger.Log(Logger.Level.Debug, "Container is empty - way");
                stringBuilder.AppendLine("Empty");
                stringBuilder.AppendLine($"{sizeLeft} of {origSize} free");
            }
            else
            {
                Logger.Log(Logger.Level.Debug, "Container Contains X Item - way");
                stringBuilder.AppendLine(itemscount + " Items - " + usedSize + " used");
                stringBuilder.AppendLine($"{sizeLeft} of {origSize} free");
            }
            return stringBuilder.ToString();
        }
    }
}
