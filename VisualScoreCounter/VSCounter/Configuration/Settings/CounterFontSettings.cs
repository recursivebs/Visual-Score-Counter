using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;


namespace VisualScoreCounter.VSCounter.Configuration {

    public class CounterFontSettings {

        public virtual float WholeNumberFontSize { get; set; } = 7.0f;
        public virtual float WholeNumberXOffset { get; set; } = 0.0f;
        public virtual float WholeNumberYOffset { get; set; } = 0.7f;
        public virtual float FractionalNumberFontSize { get; set; } = 3.5f;
        public virtual float FractionalNumberXOffset { get; set; } = 0.0f;
        public virtual float FractionalNumberYOffset { get; set; } = -3.0f;

    }

}
