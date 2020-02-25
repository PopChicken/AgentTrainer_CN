using AIChara;
using AIProject;
using AIProject.Definitions;
using CharaCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Sickness = AIProject.SaveData.Sickness;

namespace AgentTrainer
{
	public partial class AgentTrainer
	{
		const string HEADER = "Agent Trainer 汉化版";

		const float MARGIN_TOP = 20f;
		const float MARGIN_BOTTOM = 10f;
		const float MARGIN_LEFT = 10f;
		const float MARGIN_RIGHT = 10f;
		const float WIDTH = 540f;
		const float HEIGHT = 460f;
		const float INNER_WIDTH = WIDTH - MARGIN_LEFT - MARGIN_RIGHT;
		const float INNER_HEIGHT = HEIGHT - MARGIN_TOP - MARGIN_BOTTOM;

		const float ENTRY_WIDTH = 200f;
		const float AGENTS_WIDTH = 120f;
		const float LABEL_WIDTH = 90f;
		const float VALUE_WIDTH = 40f;

		static Rect rect = new Rect(
			Screen.width - WIDTH,
			(Screen.height - HEIGHT) / 2,
			WIDTH,
			HEIGHT
		);
		static Rect innerRect = new Rect(
			MARGIN_LEFT,
			MARGIN_TOP,
			INNER_WIDTH,
			INNER_HEIGHT
		);
		static Rect dragRect = new Rect(0f, 0f, WIDTH, 20f);

		GUIStyle sectionLabelStyle;
		GUIStyle selectedButtonStyle;

		readonly Dictionary<Desire.Type, int> desireTable = typeof(Desire).GetField(
			"_desireKeyTable",
			BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
		).GetValue(null) as Dictionary<Desire.Type, int>;

		readonly Dictionary<string, string> TransDic = new Dictionary<string, string>() {
			{"temperature", "体温"},
			{"mood", "心情"},
			{"hunger", "饱腹度"},
			{"physical", "精力"},
			{"life", "生活"},
			{"motivation", "干劲"},
			{"immoral", "想要h的心情"},
			{"toilet", "如厕"},
			{"bath", "洗澡"},
			{"sleep", "睡觉"},
			{"eat", "食欲"},
			{"break", "休息"},
			{"gift", "送礼"},
			{"want", "渴望"},
			{"lonely", "孤独"},
			{"h", "H欲"},
			{"dummy", "糊涂"},
			{"hunt", "钓鱼"},
			{"game", "玩耍"},
			{"cook", "烹饪"},
			{"animal", "与小动物玩耍"},
			{"location", "想去的位置"},
			{"drink", "口渴"},
			{"pheromone", "女子力"},
			{"reliability", "信赖"},
			{"reason", "人间性"},
			{"instinct", "本能"},
			{"dirty", "变态"},
			{"wariness", "警戒"},
			{"darkness", "暗"},
			{"sociability", "社交"},
			{"called", "被呼叫" },
			{"normal", "平常"},
			{"date", "约会"},
			{"onbu", "外出（Onbu?）"},
			{"idle", "闲逛"},
			{"searchbath", "找地方洗澡"},
			{"searchtoilet", "找地方上厕所"},
			{"endtaskeat", "收拾饭桌"},
			{"searchactor", "寻找玩家"},
			{"withplayer", "和玩家一块"},
			{"withagent", "和女孩子一块"},
			{"withmerchant", "和旅行商人一块"},
			{"searchgather", "前去收集"},
			{"searchgift", "寻找礼物"},
			{"endtaskgift", "找到礼物了！"},
			{"searchbreak", "找地方休息"},
			{"searchcook", "找地方做饭"},
			{"searchsleep", "找地方睡觉"},
			{"endtaskgimme", "给完东西了"},
			{"searchh", "找玩家爱爱"},
			{"endtaskmasturbation", "一切都索然无味"},
			{"escape", "逃跑"},
			{"endtaskh", "抽支事后烟"},
			{"chaselesbianh", "找妹子百合"},
			{"endtasklesbianmerchanth", "百合结束"},
			{"reverserape", "女上男"},
			{"endtaskitembox", "停止翻找物品箱"},
			{"steal", "偷窃"},
			{"searcheat", "找吃的"},
			{"aftercook", "感受自己的厨艺"},
			{"shallowsleep", "浅睡"},
			{"endtasksleep", "起床"},
			{"searchanimal", "找小动物"},
			{"endtaskpetanimal", "和小动物的游戏结束"},
			{"searchgame", "找乐子"},
			{"endtaskgame", "结束游戏时间"},
			{"searchplayertotalk", "找玩家聊天"},
			{"endtasktalktoplayer", "结束和玩家的交谈"},
			{"endtasktalk", "结束当前交谈"},
			{"receivetalk", "收到聊天请求"},
			{"fight", "决斗"},
			{"receivefight", "受到挑战书"},
			{"endtaskwildanimal", "结束和小动物的互动"},
			{"encounter", "偶遇"},
			{"faint", "昏迷"},
			{"cold2a", "感冒2A"},
			{"cold2b", "感冒2B"},
			{"cold2bmedicated", "感冒2B被医治"},
			{"cold3a", "感冒3A"},
			{"cold3b", "感冒3B"},
			{"cold3bmedicated", "感冒3B被医治"},
			{"overworka", "虚脱A"},
			{"overworkb", "虚脱B"},
			{"cure", "治愈中"},
			{"searchlocation", "前往目的地"},
			{"endtasklocation", "到达目的地"},
			{"endtasksleepafterdate", "约会后起床"},
			{"walkwithagent", "与其他少女一起散步"},
			{"endtaskgamewithagent", "结束与其他少女的玩耍"},
			{"endtasktoilet", "冲厕所中"},
			{"giftforceencounter", "送礼物给强硬的玩家"},
			{"giftstandby", "送礼物给旁人"},
			{"searchgimme", "求饶"},
			{"walkwithagentfollow", "和其他少女边走边聊"},
			{"chasetotalk", "寻求谈话"},
			{"chasetopairwalk", "寻求手挽手一起走"},
			{"endtaskgather", "结束收集"},
			{"searchmasturbation", "找地方用手解决"},
			{"gotowarditembox", "找储物箱"},
			{"endtaskgamethere", "结束在某地的游戏"},
			{"endtaskeatthere", "在某地吃完了"},
			{"searchitemforeat", "找东西吃"},
			{"endtaskgatherforeat", "找到食物了"},
			{"searcheatspot", "找餐桌"},
			{"searchrevrape", "推倒玩家"},
			{"endtaskpeeing", "在穿胖次"},
			{"searchdrink", "找水喝"},
			{"searchitemfordrink", "找饮料"},
			{"endtaskgatherfordrink", "拿到了饮用水"},
			{"searchdrinkspot", "找地方喝水"},
			{"endtaskdrink", "喝完水了"},
			{"endtaskdrinkthere", "在某地喝了水"},
			{"endtaskdressin", "穿好衣服了"},
			{"endtaskbath", "洗完澡了"},
			{"endtaskdressout", "打扮完了"},
			{"endtaskcook", "做完饭了"},
			{"endtaskbreak", "休息够了"},
			{"endtasksleeptogether", "一起睡够了"},
			{"discusslesbianh", "讨论百合H"},
			{"gotolesbianspot", "前往百合作案现场"},
			{"gotolesbianspotfollow", "跟着去百合作案现场"},
			{"endtasklesbianh", "百合结束"},
			{"endtasksecondsleep", "第二觉睡醒"},
			{"wokenup", "醒来"},
			{"photoencounter", "摆拍"},
			{"foundpeeping", "在如厕"},
			{"gotobath", "去洗澡"},
			{"gotodressout", "去打扮"},
			{"waitforcalled", "等待被呼叫"},
			{"ovation", "鼓掌欢迎"},
			{"gotowardfacialwash", "去洗脸"},
			{"endtaskfacialwash", "洗完脸了"},
			{"endtasksteal", "扒窃得手"},
			{"gotowardchestinsearchloop", "持续收集物品"},
			{"endtaskchestinsearchloop", "结束持续收集"},
			{"gotoclothchange", "去换衣服"},
			{"endtaskclothchange", "换完衣服了"},
			{"gotorestorecloth", "去穿衣服"},
			{"endtaskrestorecloth", "穿好衣服了"},
			{"gotohandwash", "去洗手"},
			{"endtaskhandwash", "洗手甩水中"},
			{"searchbirthdaygift", "寻找生日礼物"},
			{"birthdaygift", "送生日礼物"},
			{"weaknessa", "虚弱A"},
			{"weaknessb", "虚弱B"},
			{"takehpoint", "绝顶了"},
			{"commonsearchbreak", "找地方一起休息"},
			{"commonbreak", "一起休息"},
			{"commongamethere", "一起游戏"},
			{"comesleeptogether", "赶来一起睡觉"},
			{"chaseyobai", "争取幽会"},
			{"invitesleep", "邀请一起睡觉"},
			{"invitesleeph", "邀请睡前爱爱"},
			{"inviteeat", "邀请吃饭"},
			{"invitebreak", "邀请一同小憩"},
			{"takesleeppoint", "睡足了"},
			{"takesleephpoint", "睡前爱爱绝顶了"},
			{"takeeatpoint", "吃饱了"},
			{"takebreakpoint", "小憩够了"},
			{"ゲッター", "物欲强"},
			{"ベイビー", "孩子气"},
			{"ドライバー", "自立、热爱收集"},
			{"コントローラー", "嗜睡"},
			{"エキサイトメント・シーカー", "探索者"},
			{"アームチェア", "自得其乐"}
		};

		readonly List<string> tabs = new List<string>()
		{
			"信息",
			"滑块"
		};
		int tab = 0;

		bool visible = false;

		string Trans2CN(string s)
		{
			if (TransDic.ContainsKey(s.ToLower()))
			{
				return TransDic[s.ToLower()];
			}
			else
			{
				return s;
			}
		}

		void OnGUI()
		{
			if (!visible)
				return;

			if (sectionLabelStyle == null)
			{
				sectionLabelStyle = new GUIStyle(GUI.skin.label)
				{
					fontStyle = FontStyle.Bold
				};

				selectedButtonStyle = new GUIStyle(GUI.skin.button)
				{
					fontStyle = FontStyle.Bold,
					normal = {
						textColor = Color.red
					},
					focused = {
						textColor = Color.red
					},
					active = {
						textColor = Color.red
					},
					hover = {
						textColor = Color.red
					},
				};
			}

			rect = GUI.Window(
				WindowID.Value,
				rect,
				Draw,
				HEADER
			);
		}

		float Draw_Contents_Slider(string label,
								   bool locked,
								   float value,
								   float min,
								   float max,
								   out bool toggle)
		{
			GUILayout.BeginHorizontal();
			{
				toggle = GUILayout.Button(
					label,
					locked ? selectedButtonStyle : GUI.skin.button,
					GUILayout.Width(LABEL_WIDTH)
				);
				value = GUILayout.HorizontalSlider(value, min, max);

				GUILayout.Label($"{value:F0}", GUILayout.Width(VALUE_WIDTH));
			}
			GUILayout.EndHorizontal();

			return value;
		}

		float Draw_Contents_Slider(Dictionary<int, float> dict,
								   int key,
								   string label,
								   float value,
								   float min,
								   float max)
		{
			bool locked = dict.ContainsKey(key);

			if (locked)
				value = dict[key];

			float next = Draw_Contents_Slider(
				label,
				locked,
				value,
				min,
				max,
				out bool toggle
			);

			if (toggle)
				if (locked)
					dict.Remove(key);
				else
					dict[key] = next;
			else if (locked)
				dict[key] = next;

			return next;
		}

		int Draw_Contents_Slider(Dictionary<int, int> dict,
								 int key,
								 string label,
								 int value,
								 float min,
								 float max)
		{
			int next = (int)Draw_Contents_Slider(
				label,
				dict.ContainsKey(key),
				value,
				min,
				max,
				out bool toggle
			);

			if (toggle)
				if (dict.ContainsKey(key))
					dict.Remove(key);
				else
					dict[key] = next;
			else if (dict.ContainsKey(key))
				dict[key] = next;

			return next;
		}

		void Draw_Sliders_Stats()
		{
			if (controller.agent == null)
				return;

			GUILayout.Label("状态", sectionLabelStyle);

			Dictionary<int, float> stats = controller.agent.AgentData.StatsTable;

			foreach (int stat in stats.Keys.ToList())
			{
				float curr = stats[stat];
				float next = Draw_Contents_Slider(
					controller.lockedStats,
					stat,
					Trans2CN(((Status.Type)stat).ToString()),
					curr,
					0f,
					100f
				);

				if (next != curr)
					stats[stat] = next;
			}
		}

		void Draw_Sliders_Flavors()
		{
			GUILayout.Label("偏好", sectionLabelStyle);

			Dictionary<int, int> flavors = controller.ChaControl.fileGameInfo.flavorState;

			foreach (int flavor in flavors.Keys.ToList())
			{
				int curr = flavors[flavor];
				int next = Draw_Contents_Slider(
					controller.lockedFlavors,
					flavor,
					Trans2CN(((FlavorSkill.Type)flavor).ToString()),
					curr,
					0f,
					9999f
				);

				if (next != curr)
					if (controller.agent != null)
						controller.agent.SetFlavorSkill(flavor, next);
					else
						SetFlavorSkill(controller.ChaControl.fileGameInfo, flavor, next);
			}
		}

		void Draw_Sliders_Desires()
		{
			if (controller.agent == null)
				return;

			GUILayout.BeginVertical(GUILayout.Width(ENTRY_WIDTH));
			{
				GUILayout.Label("欲望", sectionLabelStyle);

				Dictionary<int, float> desires = controller.agent.AgentData.DesireTable;

				foreach (KeyValuePair<Desire.Type, int> desire in desireTable)
				{
					int key = desire.Value;
					float curr = desires[key];
					float next = Draw_Contents_Slider(
						controller.lockedDesires,
						key,
						Trans2CN(desire.Key.ToString()),
						curr,
						0f,
						100f
					);

					if (next != curr)
						desires[key] = next;
				}
			}
			GUILayout.EndVertical();
		}

		void Draw_Content_Sliders()
		{
			GUILayout.BeginVertical(GUILayout.Width(ENTRY_WIDTH));
			{
				Draw_Sliders_Stats();
				Draw_Sliders_Flavors();
			}
			GUILayout.EndVertical();

			Draw_Sliders_Desires();
		}

		float Draw_Info_Slider(string label,
							   float value,
							   float min,
							   float max,
							   Func<float, string> func = null,
							   bool drawValue = true)
		{
			GUILayout.BeginHorizontal();
			{
				if (drawValue)
				{
					label = $"{label}:\n";

					if (func != null)
						label += func(value);
					else
						label += value;
				}

				GUILayout.Label(label, GUILayout.Width(LABEL_WIDTH));

				value = GUILayout.HorizontalSlider(value, min, max);
			}
			GUILayout.EndHorizontal();

			return value;
		}

		void Draw_Info_Sliders()
		{
			ChaFileGameInfo fileGameInfo = controller.ChaControl.fileGameInfo;
			Dictionary<int, string> lifestyles = Lifestyle.LifestyleName;

			GUILayout.BeginVertical(GUILayout.Width(ENTRY_WIDTH));
			{
				GUILayout.Label("滑块", sectionLabelStyle);

				int phase = (int)Draw_Info_Slider(
					"好感阶段",
					fileGameInfo.phase,
					0,
					3,
					v => (int)v + 1 + " 心心"
				);

				if (phase != fileGameInfo.phase)
					fileGameInfo.phase = phase;

				int lifestyle = (int)Draw_Info_Slider(
					"生活方式",
					fileGameInfo.lifestyle,
					-1,
					lifestyles.Count - 1,
					v => (int)v == -1 ?
							"无" :
							lifestyles.ContainsKey((int)v) ?
								Trans2CN(lifestyles[(int)v]) :
								"未知"
				);

				if (lifestyle != fileGameInfo.lifestyle)
					fileGameInfo.lifestyle = lifestyle;

				int favoritePlace = (int)Draw_Info_Slider(
					"最喜欢的地方",
					fileGameInfo.favoritePlace,
					0,
					11
				);

				if (favoritePlace != fileGameInfo.favoritePlace)
					fileGameInfo.favoritePlace = favoritePlace;

				GUILayout.Label("着装状态", sectionLabelStyle);

				for (int i = 0; i < CATEGORY.Length; i++)
				{
					int curr = controller.ChaControl.fileStatus.clothesState[i];
					int next = (int)Draw_Info_Slider(CATEGORY[i], curr, 0, 2, drawValue: false);

					if (curr != next)
						controller.ChaControl.SetClothesState(i, (byte)next);
				}
			}
			GUILayout.EndVertical();
		}

		void Draw_Info_Texts()
		{
			if (controller.agent == null)
				return;

			AgentActor agent = controller.agent;
			PlayerActor player = Manager.Map.Instance.Player;
			Vector3 pos = controller.agent.Position;

			Sickness sick = agent.AgentData.SickState;
			string meds = sick.UsedMedicine ? "; 已治疗" : "";
			double duration = sick.Duration.TotalSeconds;
			string time = duration > 0 ? $"; {duration.ToString()}s" : "";

			GUILayout.Label("信息", sectionLabelStyle);

			GUILayout.BeginVertical(GUILayout.Width(ENTRY_WIDTH));
			{
				GUILayout.Label($"位置:\n{(int)pos.x}, {(int)pos.y}, {(int)pos.z}");
				GUILayout.Label($"与玩家的距离:\n{(int)Vector3.Distance(player.Position, pos)}");
				GUILayout.Label($"速度:\n{agent.NavMeshAgent.speed}");
				GUILayout.Label($"偏好属性总和:\n{controller.ChaControl.fileGameInfo.totalFlavor}");
				GUILayout.Label("状态:\n" + Trans2CN(agent.StateType.ToString()));
				GUILayout.Label($"现在正在:\n" + Trans2CN(agent.Mode.ToString()));
				GUILayout.Label($"疾病:\n{sick.Name}{time}{meds}");
			}
			GUILayout.EndVertical();
		}

		void Draw_Content_Info()
		{
			Draw_Info_Sliders();
			Draw_Info_Texts();
		}

		void Draw_Entries_Content()
		{
			switch (tab)
			{
				case 0:
					Draw_Content_Info();
					break;

				case 1:
					Draw_Content_Sliders();
					break;
			}
		}

		void Draw_Entries()
		{
			if (controller == null)
				return;

			Draw_Entries_Content();
		}

		void Draw_Tabs()
		{
			if (controller == null)
				return;

			GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
			{
				GUILayout.Label("标签", sectionLabelStyle);

				for (int i = 0; i < tabs.Count; i++)
					if (GUILayout.Button(
						tabs[i],
						i == tab ?
							selectedButtonStyle :
							GUI.skin.button
						))
					{
						tab = i;
						break;
					}
			}
			GUILayout.EndVertical();
		}

		void Draw_Agents()
		{
			GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
			{
				GUILayout.Label("人物", sectionLabelStyle);

				foreach (StatsController controller in controllers)
				{
					if (controller.ChaControl == null)
					{
						if (this.controller == controller)
							this.controller = null;

						controllersDump.Add(controller);
						continue;
					}

					string label;

					if (CustomBase.IsInstance() &&
						CustomBase.Instance.chaCtrl == controller.ChaControl)
						label = "[新人物]";
					else if (controller.agent != null)
						label = $"[{controller.id}]: {controller.agent.CharaName}";
					else
						label = $"[?]: {controller.ChaControl.chaFile.charaFileName}";

					GUIStyle style =
						this.controller == controller ? selectedButtonStyle : GUI.skin.button;

					if (GUILayout.Button(label, style, GUILayout.Width(AGENTS_WIDTH)))
						this.controller = controller;
				}
			}
			GUILayout.EndVertical();
		}

		void Draw(int id)
		{
			GUI.DragWindow(dragRect);
			GUILayout.BeginArea(innerRect);
			{
				GUILayout.BeginHorizontal();
				{
					GUILayout.BeginVertical(GUILayout.Width(AGENTS_WIDTH));
					{
						Draw_Agents();
						Draw_Tabs();
					}
					GUILayout.EndVertical();

					Draw_Entries();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndArea();
		}
	}
}
