using HarmonyLib;

namespace StorageInfo_BZ.Patches
{
    [HarmonyPatch(typeof(StorageContainer))]
    [HarmonyPatch(nameof(StorageContainer.OnHandHover))]
    public static class StorageContainer_OnHandHover_Patch
    {
        public static bool Prefix(StorageContainer __instance)
        {
            OnHandHover_prefix(__instance);
            return false;
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
    }
}
