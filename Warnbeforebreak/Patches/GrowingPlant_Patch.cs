using HarmonyLib;

namespace Warnbeforebreak.Patches
{
    [HarmonyPatch(typeof(GrowingPlant))]
    [HarmonyPatch(nameof(GrowingPlant.OnHandHover))]
    public static class GrowingPlant__Patch
    {
        [HarmonyPostfix]
        private static void Postfix(GUIHand __instance)
        {
            HandReticle.main.SetText(HandReticle.TextType.HandSubscript, GetInventoryleftspace(),false,GameInput.Button.None);
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
}