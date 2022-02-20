using BeatSaberMarkupLanguage.Attributes;
using VisualScoreCounter.Core.Configuration;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VisualScoreCounter.VSCounter.Configuration
{
	class CounterConfigController
	{
		private CounterSettings settings => PluginConfig.Instance.CounterSettings;

		[UIValue("PercentageRingShowsNextColor")]
		public virtual bool PercentageRingShowsNextColor
		{
			get { return settings.PercentageRingShowsNextColor; }
			set { settings.PercentageRingShowsNextColor = value; }
		}

		// Percentage Color Settings

		[UIValue("Color_100")]
		public virtual Color Color_100
		{
			get { return settings.PercentageColorSettings.Color_100; }
			set { settings.PercentageColorSettings.Color_100 = value; }
		}

		[UIValue("Color_99")]
		public virtual Color Color_99
		{
			get { return settings.PercentageColorSettings.Color_99; }
			set { settings.PercentageColorSettings.Color_99 = value; }
		}

		[UIValue("Color_98")]
		public virtual Color Color_98
		{
			get { return settings.PercentageColorSettings.Color_98; }
			set { settings.PercentageColorSettings.Color_98 = value; }
		}

		[UIValue("Color_97")]
		public virtual Color Color_97
		{
			get { return settings.PercentageColorSettings.Color_97; }
			set { settings.PercentageColorSettings.Color_97 = value; }
		}

		[UIValue("Color_96")]
		public virtual Color Color_96
		{
			get { return settings.PercentageColorSettings.Color_96; }
			set { settings.PercentageColorSettings.Color_96 = value; }
		}

		[UIValue("Color_95")]
		public virtual Color Color_95
		{
			get { return settings.PercentageColorSettings.Color_95; }
			set { settings.PercentageColorSettings.Color_95 = value; }
		}

		[UIValue("Color_94")]
		public virtual Color Color_94
		{
			get { return settings.PercentageColorSettings.Color_94; }
			set { settings.PercentageColorSettings.Color_94 = value; }
		}

		[UIValue("Color_93")]
		public virtual Color Color_93
		{
			get { return settings.PercentageColorSettings.Color_93; }
			set { settings.PercentageColorSettings.Color_93 = value; }
		}

		[UIValue("Color_92")]
		public virtual Color Color_92
		{
			get { return settings.PercentageColorSettings.Color_92; }
			set { settings.PercentageColorSettings.Color_92 = value; }
		}

		[UIValue("Color_91")]
		public virtual Color Color_91
		{
			get { return settings.PercentageColorSettings.Color_91; }
			set { settings.PercentageColorSettings.Color_91 = value; }
		}

		[UIValue("Color_90")]
		public virtual Color Color_90
		{
			get { return settings.PercentageColorSettings.Color_90; }
			set { settings.PercentageColorSettings.Color_90 = value; }
		}

		[UIValue("Color_89")]
		public virtual Color Color_89
		{
			get { return settings.PercentageColorSettings.Color_89; }
			set { settings.PercentageColorSettings.Color_89 = value; }
		}

		[UIValue("Color_88")]
		public virtual Color Color_88
		{
			get { return settings.PercentageColorSettings.Color_88; }
			set { settings.PercentageColorSettings.Color_88 = value; }
		}

		[UIValue("Color_80")]
		public virtual Color Color_80
		{
			get { return settings.PercentageColorSettings.Color_80; }
			set { settings.PercentageColorSettings.Color_80 = value; }
		}

		[UIValue("Color_65")]
		public virtual Color Color_65
		{
			get { return settings.PercentageColorSettings.Color_65; }
			set { settings.PercentageColorSettings.Color_65 = value; }
		}

		[UIValue("Color_50")]
		public virtual Color Color_50
		{
			get { return settings.PercentageColorSettings.Color_50; }
			set { settings.PercentageColorSettings.Color_50 = value; }
		}

		[UIValue("Color_35")]
		public virtual Color Color_35
		{
			get { return settings.PercentageColorSettings.Color_35; }
			set { settings.PercentageColorSettings.Color_35 = value; }
		}

		[UIValue("Color_20")]
		public virtual Color Color_20
		{
			get { return settings.PercentageColorSettings.Color_20; }
			set { settings.PercentageColorSettings.Color_20 = value; }
		}

		[UIValue("Color_0")]
		public virtual Color Color_0
		{
			get { return settings.PercentageColorSettings.Color_0; }
			set { settings.PercentageColorSettings.Color_0 = value; }
		}

	}

}
