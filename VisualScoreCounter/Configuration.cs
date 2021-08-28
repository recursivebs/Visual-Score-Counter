using IPA.Config.Stores;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace VisualScoreCounter
{
    class Configuration
    {
        public static Configuration Instance { get; set; }
        public virtual bool percentageRingShowsNextRankColor { get; set; } = true;
        public virtual Color Color_100 { get; set; } = new Color(1.0f, 1.0f, 1.0f);
        public virtual Color Color_99 { get; set; } = new Color(1.0f, 1.0f, 0.68846f);
        public virtual Color Color_98 { get; set; } = new Color(0.1371948f, 0.9527207f, 0.250269f);
        public virtual Color Color_97 { get; set; } = new Color(0.9107788f, 1.0f, 0.0f);
        public virtual Color Color_96 { get; set; } = new Color(1.0f, 0.5137255f, 0.07058824f);
        public virtual Color Color_95 { get; set; } = new Color(0.9706014f, 0.7994141f, 0.9706014f);
        public virtual Color Color_94 { get; set; } = new Color(0.4726773f, 0.02532541f, 0.8719445f);
        public virtual Color Color_93 { get; set; } = new Color(0.9054167f, 1.0f, 0.0f);
        public virtual Color Color_92 { get; set; } = new Color(0.3497706f, 1.0f, 0.3794441f);
        public virtual Color Color_91 { get; set; } = new Color(0.4745098f, 0.6597694f, 1.0f);
        public virtual Color Color_90 { get; set; } = new Color(0.7176471f, 0.9803922f, 1.0f);
        public virtual Color Color_89 { get; set; } = Color.white;
        public virtual Color Color_88 { get; set; } = Color.white;
        public virtual Color Color_80 { get; set; } = Color.gray;
        public virtual Color Color_65 { get; set; } = Color.green;
        public virtual Color Color_50 { get; set; } = Color.yellow;
        public virtual Color Color_35 { get; set; } = new Color(1.0f, 0.5f, 0.0f);
        public virtual Color Color_20 { get; set; } = Color.red;
        public virtual Color Color_0 { get; set; } = Color.red;
    }
}

