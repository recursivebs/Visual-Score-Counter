using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;


namespace VisualScoreCounter.VSCounter.Configuration {

    public class CounterFontSettings {

        public virtual float WholeNumberFontSize { get; set; } = Default_WholeNumberFontSize;
        public virtual float WholeNumberXOffset { get; set; } = Default_WholeNumberXOffset;
        public virtual float WholeNumberYOffset { get; set; } = Default_WholeNumberYOffset;
        public virtual float FractionalNumberFontSize { get; set; } = Default_FractionalNumberFontSize;
        public virtual float FractionalNumberXOffset { get; set; } = Default_FractionalNumberXOffset;
        public virtual float FractionalNumberYOffset { get; set; } = Default_FractionalNumberYOffset;
        public virtual bool BloomFont { get; set; } = Default_BloomFont;

        public void Reset() {
            WholeNumberFontSize = Default_WholeNumberFontSize;
            WholeNumberXOffset = Default_WholeNumberXOffset;
            WholeNumberYOffset = Default_WholeNumberYOffset;
            FractionalNumberFontSize = Default_FractionalNumberFontSize;
            FractionalNumberXOffset = Default_FractionalNumberXOffset;
            FractionalNumberYOffset = Default_FractionalNumberYOffset;
            BloomFont = Default_BloomFont;
        }

        // Defaults
        public static float Default_WholeNumberFontSize = 7.0f;
        public static float Default_WholeNumberXOffset = 0.0f;
        public static float Default_WholeNumberYOffset = 0.7f;
        public static float Default_FractionalNumberFontSize = 3.5f;
        public static float Default_FractionalNumberXOffset = 0.0f;
        public static float Default_FractionalNumberYOffset = -3.0f;
        public static bool Default_BloomFont = false;

    }

}
