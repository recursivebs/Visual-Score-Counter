using IPA.Config.Stores;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace VisualScoreCounter
{
    class Configuration
    {
        public static Configuration Instance { get; set; }
        public virtual bool showPercentageRing { get; set; } = true;
        public virtual bool percentageRingShowsNextRankColor { get; set; } = true;
        public virtual bool percentMode { get; set; } = false;
        public virtual bool showScore { get; set; } = true;
        public virtual Color Color_100 { get; set; } = new Color(1.0f, 69.0f/255.0f, 0.0f);
        public virtual Color Color_99 { get; set; } = new Color(1.0f, 69.0f/255.0f, 0.0f);
        public virtual Color Color_98 { get; set; } = new Color(1.0f, 166.0f / 255.0f, 0.0f);
        public virtual Color Color_97 { get; set; } = new Color(1.0f, 77.0f / 255.0f, 0.0f);
        public virtual Color Color_96 { get; set; } = new Color(227.0f / 255.0f, 18.0f / 255.0f, 113.0f / 255.0f);
        public virtual Color Color_95 { get; set; } = new Color(245.0f / 255.0f, 10.0f / 255.0f, 227.0f / 255.0f);
        public virtual Color Color_94 { get; set; } = new Color(162.0f / 255.0f, 13.0f / 255.0f, 242.0f / 255.0f);
        public virtual Color Color_93 { get; set; } = new Color(11.0f / 255.0f, 34.0f / 255.0f, 244.0f / 255.0f);
        public virtual Color Color_92 { get; set; } = new Color(0.0f, 198.0f/255.0f, 1.0f);
        public virtual Color Color_91 { get; set; } = new Color(125.0f/255.0f, 245.0f/255.0f, 255.0f/255.0f);
        public virtual Color Color_90 { get; set; } = new Color(183.0f/255.0f, 250.0f/255.0f, 255.0f/255.0f);
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

