﻿using BeatSaberMarkupLanguage.Attributes;
using VisualScoreCounter.Core.Configuration;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VisualScoreCounter.VSCounter.Configuration {

	class CounterConfigController : INotifyPropertyChanged {

		private CounterSettings settings => PluginConfig.Instance.CounterSettings;

#pragma warning disable CS8618
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS8618

		private void RaisePropertyChanged([CallerMemberName] string propertyName = "") {
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		[UIAction("#reset-settings")]
		public void ResetSettings() {
			settings.RingScale = 1.0f;
			RaisePropertyChanged(nameof(RingScale));
			settings.BloomRing = false;
			RaisePropertyChanged(nameof(BloomRing));
			settings.PercentageRingShowsNextColor = true;
			RaisePropertyChanged(nameof(PercentageRingShowsNextColor));
			settings.HideBaseGameRankDisplay = true;
			RaisePropertyChanged(nameof(HideBaseGameRankDisplay));
			settings.CounterXOffset = 0.0f;
			RaisePropertyChanged(nameof(CounterXOffset));
			settings.CounterYOffset = 0.0f;
			RaisePropertyChanged(nameof(CounterYOffset));
        }


		[UIAction("#reset-colors")]
		public void ResetColors() {
			settings.PercentageColorSettings.Reset();
			RaisePropertyChanged(nameof(Color_100));
			RaisePropertyChanged(nameof(Color_99));
			RaisePropertyChanged(nameof(Color_98));
			RaisePropertyChanged(nameof(Color_97));
			RaisePropertyChanged(nameof(Color_96));
			RaisePropertyChanged(nameof(Color_95));
			RaisePropertyChanged(nameof(Color_94));
			RaisePropertyChanged(nameof(Color_93));
			RaisePropertyChanged(nameof(Color_92));
			RaisePropertyChanged(nameof(Color_91));
			RaisePropertyChanged(nameof(Color_90));
			RaisePropertyChanged(nameof(Color_89));
			RaisePropertyChanged(nameof(Color_89));
			RaisePropertyChanged(nameof(Color_80));
			RaisePropertyChanged(nameof(Color_65));
			RaisePropertyChanged(nameof(Color_50));
			RaisePropertyChanged(nameof(Color_35));
			RaisePropertyChanged(nameof(Color_20));
			RaisePropertyChanged(nameof(Color_0));
        }


		[UIAction("#reset-font")]
		public void ResetFont() {
			settings.CounterFontSettings.Reset();
			RaisePropertyChanged(nameof(BloomFont));
			RaisePropertyChanged(nameof(FractionalNumberFontSize));
			RaisePropertyChanged(nameof(FractionalNumberXOffset));
			RaisePropertyChanged(nameof(FractionalNumberYOffset));
			RaisePropertyChanged(nameof(WholeNumberFontSize));
			RaisePropertyChanged(nameof(WholeNumberXOffset));
			RaisePropertyChanged(nameof(WholeNumberYOffset));
        }

		[UIValue("bloom-ring")]
		public virtual bool BloomRing
		{
			get { return settings.BloomRing; }
			set { settings.BloomRing = value; }
		}

		[UIValue("bloom-font")]
		public virtual bool BloomFont
		{
			get { return settings.CounterFontSettings.BloomFont; }
			set { settings.CounterFontSettings.BloomFont = value; }
		}

		[UIValue("counter-x-offset")]
		public virtual float CounterXOffset {
			get { return settings.CounterXOffset; }
			set { settings.CounterXOffset = value; }
		}


		[UIValue("counter-y-offset")]
		public virtual float CounterYOffset {
			get { return settings.CounterYOffset; }
			set { settings.CounterYOffset = value; }
		}


		[UIValue("HideBaseGameRankDisplay")]
		public virtual bool HideBaseGameRankDisplay {
			get { return settings.HideBaseGameRankDisplay; }
			set { settings.HideBaseGameRankDisplay = value; }
		}


		[UIValue("ring-scale")]
		public virtual float RingScale {
			get { return settings.RingScale; }
			set { settings.RingScale = value; }
		}


		[UIValue("PercentageRingShowsNextColor")]
		public virtual bool PercentageRingShowsNextColor {
			get { return settings.PercentageRingShowsNextColor; }
			set { settings.PercentageRingShowsNextColor = value; }
		}


        [UIValue("whole-number-font-size")]
		public virtual float WholeNumberFontSize {
			get { return settings.CounterFontSettings.WholeNumberFontSize; }
			set { settings.CounterFontSettings.WholeNumberFontSize = value; }
		}


        [UIValue("whole-number-x-offset")]
		public virtual float WholeNumberXOffset {
			get { return settings.CounterFontSettings.WholeNumberXOffset; }
			set { settings.CounterFontSettings.WholeNumberXOffset = value; }
		}


        [UIValue("whole-number-y-offset")]
		public virtual float WholeNumberYOffset {
			get { return settings.CounterFontSettings.WholeNumberYOffset; }
			set { settings.CounterFontSettings.WholeNumberYOffset = value; }
		}


        [UIValue("fractional-number-font-size")]
		public virtual float FractionalNumberFontSize {
			get { return settings.CounterFontSettings.FractionalNumberFontSize; }
			set { settings.CounterFontSettings.FractionalNumberFontSize = value; }
		}


        [UIValue("fractional-number-x-offset")]
		public virtual float FractionalNumberXOffset {
			get { return settings.CounterFontSettings.FractionalNumberXOffset; }
			set { settings.CounterFontSettings.FractionalNumberXOffset = value; }
		}


        [UIValue("fractional-number-y-offset")]
		public virtual float FractionalNumberYOffset {
			get { return settings.CounterFontSettings.FractionalNumberYOffset; }
			set { settings.CounterFontSettings.FractionalNumberYOffset = value; }
		}


		// Percentage Color Settings
		[UIValue("Color_100")]
		public virtual Color Color_100 {
			get { return settings.PercentageColorSettings.Color_100; }
			set { settings.PercentageColorSettings.Color_100 = value; }
		}


		[UIValue("Color_99")]
		public virtual Color Color_99 {
			get { return settings.PercentageColorSettings.Color_99; }
			set { settings.PercentageColorSettings.Color_99 = value; }
		}


		[UIValue("Color_98")]
		public virtual Color Color_98 {
			get { return settings.PercentageColorSettings.Color_98; }
			set { settings.PercentageColorSettings.Color_98 = value; }
		}


		[UIValue("Color_97")]
		public virtual Color Color_97 {
			get { return settings.PercentageColorSettings.Color_97; }
			set { settings.PercentageColorSettings.Color_97 = value; }
		}

		
		[UIValue("Color_96")]
		public virtual Color Color_96 {
			get { return settings.PercentageColorSettings.Color_96; }
			set { settings.PercentageColorSettings.Color_96 = value; }
		}


		[UIValue("Color_95")]
		public virtual Color Color_95 {
			get { return settings.PercentageColorSettings.Color_95; }
			set { settings.PercentageColorSettings.Color_95 = value; }
		}


		[UIValue("Color_94")]
		public virtual Color Color_94 {
			get { return settings.PercentageColorSettings.Color_94; }
			set { settings.PercentageColorSettings.Color_94 = value; }
		}


		[UIValue("Color_93")]
		public virtual Color Color_93 {
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
		public virtual Color Color_90 {
			get { return settings.PercentageColorSettings.Color_90; }
			set { settings.PercentageColorSettings.Color_90 = value; }
		}

		
		[UIValue("Color_89")]
		public virtual Color Color_89 {
			get { return settings.PercentageColorSettings.Color_89; }
			set { settings.PercentageColorSettings.Color_89 = value; }
		}


		[UIValue("Color_88")]
		public virtual Color Color_88 {
			get { return settings.PercentageColorSettings.Color_88; }
			set { settings.PercentageColorSettings.Color_88 = value; }
		}


		[UIValue("Color_80")]
		public virtual Color Color_80 {
			get { return settings.PercentageColorSettings.Color_80; }
			set { settings.PercentageColorSettings.Color_80 = value; }
		}


		[UIValue("Color_65")]
		public virtual Color Color_65 {
			get { return settings.PercentageColorSettings.Color_65; }
			set { settings.PercentageColorSettings.Color_65 = value; }
		}


		[UIValue("Color_50")]
		public virtual Color Color_50 {
			get { return settings.PercentageColorSettings.Color_50; }
			set { settings.PercentageColorSettings.Color_50 = value; }
		}


		[UIValue("Color_35")]
		public virtual Color Color_35 {
			get { return settings.PercentageColorSettings.Color_35; }
			set { settings.PercentageColorSettings.Color_35 = value; }
		}


		[UIValue("Color_20")]
		public virtual Color Color_20 {
			get { return settings.PercentageColorSettings.Color_20; }
			set { settings.PercentageColorSettings.Color_20 = value; }
		}


		[UIValue("Color_0")]
		public virtual Color Color_0 {
			get { return settings.PercentageColorSettings.Color_0; }
			set { settings.PercentageColorSettings.Color_0 = value; }
		}

	}

}
