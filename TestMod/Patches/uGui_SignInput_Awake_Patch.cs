//default
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//added
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace TestMod.Patches
{
    [HarmonyPatch(typeof(uGUI_SignInput))]
    [HarmonyPatch("Awake")]
    public static class uGui_SignInput_Awake_Patch
    {
		private static void Postfix(uGUI_SignInput __instance)
        {
			QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Desp_TestMod_SN Start Postfix");
			if (IsOnSmallLocker(__instance))
			{
				QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Desp_TestMod_SN Postfix IsOnSmallLocker");
				PatchSmallLocker(__instance);
				QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Desp_TestMod_SN Postfix IsOnSmallLocker Patched");
			}
			else if (IsOnSign(__instance))
			{
				QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Desp_TestMod_SN Postfix IsOnSign");
				PatchSign(__instance);
				QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Desp_TestMod_SN Postfix IsOnSign Patched");
			}
		}

		private static bool IsOnSmallLocker(uGUI_SignInput __instance)
		{
			QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Desp_TestMod_SN IsOnSmallLocker");
			var root = __instance.gameObject.GetComponentInParent<Constructable>();
			return root.gameObject.name.Contains("SmallLocker");
		}
		private static bool IsOnSign(uGUI_SignInput __instance)
		{
			QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Desp_TestMod_SN IsOnSmallLocker");
			var root = __instance.gameObject.GetComponentInParent<Constructable>();
			return root.gameObject.name.Contains("Sign");
		}

		private const float TextFieldHeight = 600;
		//public int SmallLockerTextLimit = 60;
		//public int SignTextLimit = 100;

		private static void PatchSmallLocker(uGUI_SignInput __instance)
		{
			QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Desp_TestMod_SN PatchSmallLocker Methode begin");
			__instance.inputField.lineType = InputField.LineType.MultiLineNewline;
			__instance.inputField.characterLimit = 60;

			var rt = __instance.inputField.transform as RectTransform;
			RectTransformExtensions.SetSize(rt, rt.rect.width, TextFieldHeight);

			GameObject.Destroy(__instance.inputField.textComponent.GetComponent<ContentSizeFitter>());
			rt = __instance.inputField.textComponent.transform as RectTransform;
			RectTransformExtensions.SetSize(rt, rt.rect.width, TextFieldHeight);

			__instance.inputField.textComponent.alignment = TextAnchor.MiddleCenter;
		}

		private static void PatchSign(uGUI_SignInput __instance)
		{
			QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Desp_TestMod_SN PatchSign Methode begin");
			__instance.inputField.characterLimit = 100;			
		}

	}
}
