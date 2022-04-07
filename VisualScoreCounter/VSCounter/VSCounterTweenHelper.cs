using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VisualScoreCounter.VSCounter
{
    internal class VSCounterTweenHelper : MonoBehaviour
    {
        public VSCounterTweenHelper()
        {
            animationTime = 0.20f;
            easeType = EaseType.InExpo;
        }

        public float animationTime { get; set; }
        public EaseType easeType { get; set; }
    }
}
