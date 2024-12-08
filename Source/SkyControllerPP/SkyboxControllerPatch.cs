﻿using System;
using System.Collections;
using HarmonyLib;
using Home.Shared;

namespace SkyControllerPP
{
	// Token: 0x0200000A RID: 10
	[HarmonyPatch(typeof(SkyboxController), "Update")]
	public class SkyboxControllerPatch
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00004104 File Offset: 0x00002304
		[HarmonyPostfix]
		public static void Postfix(SkyboxController __instance)
		{
			if (__instance.isActiveAndEnabled && !SkyboxControllerPatch.Last)
			{
				if (!(ApplicationController.ApplicationContext.navigationController == null) && (ApplicationController.ApplicationContext.navigationController.IsSceneLoaded(SceneType.GAME) || ApplicationController.ApplicationContext.navigationController.IsSceneLoaded(SceneType.GAME_BASE)))
				{
					SkyInfo.Instance = __instance.gameObject.AddComponent<SkyControllerPlus>();
					SkyInfo.Phase = "Day";
				}
				else
				{
					SkyInfo.Instance = __instance.gameObject.AddComponent<SkyControllerPlus>();
					SkyInfo.Instance.Famine = false;
					SkyInfo.Instance.Pest = false;
					SkyInfo.Instance.War = false;
					SkyInfo.Instance.Death = false;
					SkyInfo.Phase = "NotGame";
					ApplicationController.ApplicationContext.StartCoroutine(SkyboxControllerPatch.SetSkyShader());
				}
			}
			SkyboxControllerPatch.Last = (__instance.isActiveAndEnabled && SkyInfo.Instance != null);
			if (SkyboxControllerPatch.LastScene != null && SkyboxControllerPatch.LastScene != ApplicationController.ApplicationContext.navigationController.CurrentSceneName)
			{
				if (ApplicationController.ApplicationContext.navigationController.IsSceneLoaded(SceneType.GAME) || ApplicationController.ApplicationContext.navigationController.IsSceneLoaded(SceneType.GAME_BASE))
				{
					SkyInfo.Phase = "Day";
					SkyInfo.Instance.UpdateSky();
				}
				else
				{
					SkyInfo.Instance.Famine = false;
					SkyInfo.Instance.Pest = false;
					SkyInfo.Instance.War = false;
					SkyInfo.Instance.Death = false;
					SkyInfo.Phase = "NotGame";
					ApplicationController.ApplicationContext.StartCoroutine(SkyboxControllerPatch.SetSkyShader());
				}
			}
			SkyboxControllerPatch.LastScene = ApplicationController.ApplicationContext.navigationController.CurrentSceneName;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000020D4 File Offset: 0x000002D4
		private static IEnumerator SetSkyShader()
		{
			yield return null;
			if (!(ApplicationController.ApplicationContext.navigationController == null) && ApplicationController.ApplicationContext.navigationController.IsSceneLoaded(SceneType.HOME))
			{
				switch (SkyInfo.Instance.GetCurrentSkyType())
				{
				case SkyInfo.SkyType.Day:
					SkyInfo.Instance.UpdateIntroClouds("Day");
					break;
				case SkyInfo.SkyType.Night:
					SkyInfo.Instance.UpdateIntroClouds("Night");
					break;
				case SkyInfo.SkyType.Dawn:
					SkyInfo.Instance.UpdateIntroClouds("Dawn");
					break;
				case SkyInfo.SkyType.Dusk:
					SkyInfo.Instance.UpdateIntroClouds("Dawn");
					break;
				case SkyInfo.SkyType.BloodMoon:
					SkyInfo.Instance.UpdateIntroClouds("BloodMoon");
					break;
				case SkyInfo.SkyType.Storm:
					SkyInfo.Instance.UpdateIntroClouds("Storm");
					break;
				case SkyInfo.SkyType.Eclipse:
					SkyInfo.Instance.UpdateIntroClouds("Invis");
					break;
				case SkyInfo.SkyType.Winter:
					SkyInfo.Instance.UpdateIntroClouds("Invis");
					break;
				}
			}
			SkyInfo.Instance.UpdateSky();
			yield break;
		}

		// Token: 0x04000023 RID: 35
		public static bool Last;

		// Token: 0x04000024 RID: 36
		public static string LastScene;
	}
}
