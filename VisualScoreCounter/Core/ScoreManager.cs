#nullable enable
using System;
using Zenject;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static PlayerSaveData;

namespace VisualScoreCounter.Core
{

    // Responsible for managing the score internally, since we can't really trust the
    // way the base game computes score.
    // Borrowing a lot of this from https://github.com/ChirpyMisha/FC-Percentage
    public class ScoreManager : IInitializable, IDisposable {

        public event EventHandler? OnScoreUpdate;

        private static double defaultPercentageAtStart = 100;
        private static double defaultPercentageAtEnd = 0;
        public int MaxScoreAtLevelStart { get; private set; }

        private double defaultPercentage;
        public int Highscore { get; private set; }

        public double PercentageTotal => CalculatePercentage(ScoreTotal, MaxScoreTotal);
        public double PercentageA => CalculatePercentage(ScoreA, MaxScoreA);
        public double PercentageB => CalculatePercentage(ScoreB, MaxScoreB);
        public int ScoreTotal => ScoreA + ScoreB;
        public int ScoreA { get; private set; }
        public int ScoreB { get; private set; }
        public int MaxScoreTotal => MaxScoreA + MaxScoreB;
        public int MaxScoreA { get; private set; }
        public int MaxScoreB { get; private set; }
        public int ScoreAtCurrentPercentage => CalculateScoreFromCurrentPercentage();

        public string SaberAColor { get; private set; } = "#FFFFFF";
        public string SaberBColor { get; private set; } = "#FFFFFF";


        public ScoreManager() {
            ResetScore();
        }

        public void Dispose() {
            return;
        }

        public void Initialize() {
            return;
        }

        private void ResetScore() {
            ScoreA = 0;
            ScoreB = 0;
            MaxScoreA = 0;
            MaxScoreB = 0;
            Highscore = 0;
            MaxScoreAtLevelStart = 0;
            defaultPercentage = defaultPercentageAtStart;
        }

        internal void NotifyOfSongEnded(int levelResultScoreModified) {
            defaultPercentage = defaultPercentageAtEnd;
            UpdateHighscore(levelResultScoreModified);
        }

        private void UpdateHighscore(int levelResultScoreModified) {
            if (levelResultScoreModified > Highscore)
            {
                Highscore = levelResultScoreModified;
            }
        }


        internal void ResetScoreManager(IDifficultyBeatmap beatmap, PlayerDataModel playerDataModel, ColorScheme colorScheme)
        {
            ResetScore();

            PlayerLevelStatsData stats = playerDataModel.playerData.GetPlayerLevelStatsData(beatmap);
            Highscore = stats.highScore;
            MaxScoreAtLevelStart = CalculateMaxScore(beatmap.beatmapData.cuttableNotesCount);

            SaberAColor = "#" + ColorUtility.ToHtmlStringRGB(colorScheme.saberAColor);
            SaberBColor = "#" + ColorUtility.ToHtmlStringRGB(colorScheme.saberBColor);

            InvokeScoreUpdate();
        }



        protected virtual void InvokeScoreUpdate() {
            LogScoreChange();
            // Create event handler
            EventHandler? handler = OnScoreUpdate;
            if (handler != null) {
                // Invoke event
                handler(this, EventArgs.Empty);
            }
        }


        public string CreatePercentageStringFormat(int decimalPrecision) {
            string percentageStringFormat = "0";
            if (decimalPrecision > 0) {
                percentageStringFormat += "." + new string('0', decimalPrecision);
            }
            return percentageStringFormat;
        }



        private int CalculateMaxScore(int noteCount) {
            if (noteCount < 14) {
                if (noteCount == 1) {
                    return 115;
                } else if (noteCount < 5) {
                    return (noteCount - 1) * 230 + 115;
                } else {
                    return (noteCount - 5) * 460 + 1035;
                }
            } else {
                return (noteCount - 13) * 920 + 4715;
            }
        }

        private double CalculatePercentage(int val, int maxVal) {
            double retVal = CalculateRatio(val, maxVal) * 100;
            return retVal;
        }

        private double CalculateRatio(int val, int maxVal) {
            double retVal = 0;
            if (maxVal == 0) {
                retVal = defaultPercentage / 100;
            } else {
                retVal = (double)val / (double)maxVal;
            }
            return retVal;
        }

        public string PercentageToString(double percentage, string decimalFormat, bool keepTrailingZeros) {

            int decimalPrecision = 0;

            if (decimalFormat.Length >= 3) {
                // A length smaller than 3 means that it contains less than 1 decimal.
                decimalPrecision = decimalFormat.Length - 2;
            }

            percentage = Math.Round(percentage, decimalPrecision);

            string result;
            if (keepTrailingZeros)
            {
                result = percentage.ToString(decimalFormat);
            } else {
                result = percentage.ToString();
            }

            result += "%";
            return result;

        }

        public string ScoreToString(int score) {
            // Format the score to norwegian notation (which uses spaces as seperator in large numbers) and then remove the decimal characters ",00" from the end.
            string scoreString = score.ToString("n", new CultureInfo("nb-NO"));
            return scoreString.Remove(scoreString.Length - 3);
        }


        private int CalculateScoreFromCurrentPercentage() {
            if (MaxScoreTotal == 0) {
                return 0;
            }
            double currentRatio = CalculateRatio(ScoreTotal, MaxScoreTotal);
            return (int)Math.Round(currentRatio * MaxScoreAtLevelStart);
        }


        private void LogScoreChange()
        {
            Plugin.Log.Debug("VisualScoreCounter : ScoreTotal: " + ScoreTotal + ", MaxScoreTotal: " + MaxScoreTotal + ", PercentageTotal: " + PercentageTotal);
        }


        internal void AddScore(ColorType colorType, int score, int comboMultiplier, int fcMultiplier)
        {

            Plugin.Log.Debug("VisualScoreCounter : AddScore | score: " + score + ", comboMultiplier: " + comboMultiplier + ", fcMultiplier: " + fcMultiplier);

            // Update score for left or right saber
            if (colorType == ColorType.ColorA) {

                double preScoreA = ScoreA;
                double preMaxScoreA = MaxScoreA;

                ScoreA += score * comboMultiplier;
                MaxScoreA += ScoreModel.kMaxCutRawScore * fcMultiplier;

            } else if (colorType == ColorType.ColorB) {

                double preScoreB = ScoreB;
                double preMaxScoreB = MaxScoreB;

                ScoreB += score * comboMultiplier;
                MaxScoreB += ScoreModel.kMaxCutRawScore * fcMultiplier;
            }

            // Inform listeners that the score has updated
            InvokeScoreUpdate();
        }

        internal void SubtractScore(ColorType colorType, int score, int comboMultiplier, int fcMultiplier, bool subtractFromMaxScore = false)
        {
            Plugin.Log.Debug("VisualScoreCounter : SubtractScore | score: " + score + ", comboMultiplier: " + comboMultiplier + ", fcMultiplier: " + fcMultiplier);
            // Update score for left or right saber
            if (colorType == ColorType.ColorA)
            {
                ScoreA -= score * comboMultiplier;
                if (subtractFromMaxScore) MaxScoreA -= ScoreModel.kMaxCutRawScore * fcMultiplier;
            }
            else if (colorType == ColorType.ColorB)
            {
                ScoreB -= score * comboMultiplier;
                if (subtractFromMaxScore) MaxScoreB -= ScoreModel.kMaxCutRawScore * fcMultiplier;
            }

            // Inform listeners that the score has updated
            InvokeScoreUpdate();
        }

        public void syncScore(int realScore, int realMaxScore)
        {
            ScoreA = Convert.ToInt32(Math.Round(realScore / 2.0d));
            ScoreB = Convert.ToInt32(Math.Round(realScore / 2.0d));
            MaxScoreA = Convert.ToInt32(Math.Round(realMaxScore / 2.0d));
            MaxScoreB = Convert.ToInt32(Math.Round(realMaxScore / 2.0d));
            InvokeScoreUpdate();
        }





    }

}
