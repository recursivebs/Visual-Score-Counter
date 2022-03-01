using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualScoreCounter.Core
{
    class ScoreHelpers
    {
        public static int CalculateMaxScore(int noteCount)
        {
            if (noteCount < 14)
            {
                if (noteCount == 1)
                {
                    return 115;
                }
                else if (noteCount < 5)
                {
                    return (noteCount - 1) * 230 + 115;
                }
                else
                {
                    return (noteCount - 5) * 460 + 1035;
                }
            }
            else
            {
                return (noteCount - 13) * 920 + 4715;
            }
        }

        public static double CalculatePercentage(int val, int maxVal)
        {
            double retVal = CalculateRatio(val, maxVal) * 100;
            retVal = Math.Round(retVal, 2);
            return retVal;
        }

        public static double CalculateRatio(int val, int maxVal)
        {
            double retVal = 0;
            if (maxVal == 0)
            {
                retVal = (double)1;
            }
            else
            {
                retVal = (double)val / (double)maxVal;
            }
            return retVal;
        }

        public static int GetMultiplierForCombo(int c)
        {
            if (c > 13)
            {
                return 8;
            }
            if (c > 5)
            {
                return 4;
            }
            if (c > 1)
            {
                return 2;
            }
            return 1;
        }


    }
}
