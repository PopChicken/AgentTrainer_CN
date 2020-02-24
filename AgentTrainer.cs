using BepInEx.Configuration;
using UnityEngine;
using BepInEx;
using KeyboardShortcut = BepInEx.Configuration.KeyboardShortcut;
using KKAPI.Chara;
using BepInEx.Harmony;

namespace AgentTrainer
{
	[BepInPlugin(GUID, Name, Version)]
	[BepInProcess("AI-Syoujyo")]
	public partial class AgentTrainer : BaseUnityPlugin
	{
		const string GUID = "com.fairbair.agenttrainer";
		const string Name = "Agent Trainer 汉化版";
		const string Version = "1.2.0";
		const string BEHAVIOR = "AgentTrainer.StatsController";

		const string SECTION_GENERAL = "General";

		static ConfigEntry<int> WindowID { get; set; }
		static ConfigEntry<KeyboardShortcut> Key { get; set; }

		void Awake()
		{
			WindowID = Config.Bind(SECTION_GENERAL, "窗口ID(__Window ID):", 23967);

			Key = Config.Bind(SECTION_GENERAL, "快捷键", new KeyboardShortcut(KeyCode.KeypadEnter));

			CharacterApi.RegisterExtraBehaviour<StatsController>(BEHAVIOR);
			HarmonyWrapper.PatchAll(typeof(AgentTrainer));
		}
	}
}