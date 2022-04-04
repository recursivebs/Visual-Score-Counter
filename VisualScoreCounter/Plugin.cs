using HarmonyLib;
using IPA;
using SiraUtil.Zenject;
using IPA.Config;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;
using Zenject;
using VisualScoreCounter.Core;
using System.Reflection;
using VisualScoreCounter.Core.Configuration;

namespace VisualScoreCounter
{
	[Plugin(RuntimeOptions.DynamicInit)]
	public class Plugin
	{
		public static Harmony harmony;

#pragma warning disable CS8618
		internal static Plugin Instance { get; private set; }
		internal static IPALogger Log { get; private set; }
#pragma warning restore CS8618

		internal static string PluginName = "VisualScoreCounter";

		[Init]
		/// <summary>
		/// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
		/// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
		/// Only use [Init] with one Constructor.
		/// </summary>
		public void Init(IPALogger logger, Zenjector zenjector)
		{
			Instance = this;
			Log = logger;

			Log.Info($"{PluginName} initialized.");
		}

		[Init]
		public void InitWithConfig(Config conf)
		{
			PluginConfig.Instance = conf.Generated<PluginConfig>();
		}

		[OnEnable]
		public void OnApplicationStart()
		{
			harmony = new Harmony("com.bluecurse.BeatSaber.VisualScoreCounter");
			harmony.PatchAll(Assembly.GetExecutingAssembly());
		}

		[OnDisable]
		public void OnApplicationQuit()
		{
			harmony.UnpatchSelf();
		}

	}
}
