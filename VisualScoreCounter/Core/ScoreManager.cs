#nullable enable
using System;
using Zenject;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using CountersPlus.Counters.Custom;
using CountersPlus.Counters.Interfaces;
using static PlayerSaveData;
using CountersPlus.Counters.NoteCountProcessors;
using System.Linq;

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

        public double PercentageTotal => ScoreHelpers.CalculatePercentage(ScoreTotal, MaxScoreTotal);
        public double PercentageA => ScoreHelpers.CalculatePercentage(ScoreA, MaxScoreA);
        public double PercentageB => ScoreHelpers.CalculatePercentage(ScoreB, MaxScoreB);
        public int ScoreTotal => ScoreA + ScoreB;
        public int ScoreA { get; private set; }
        public int ScoreB { get; private set; }
        public int MaxScoreTotal => MaxScoreA + MaxScoreB;
        public int MaxScoreA { get; private set; }
        public int MaxScoreB { get; private set; }
        public int ScoreAtCurrentPercentage => CalculateScoreFromCurrentPercentage();

        [Inject] private GameplayCoreSceneSetupData setupData;
        [Inject] private NoteCountProcessor noteCountProcessor;

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

        internal void ResetScoreManager(IDifficultyBeatmap beatmap, PlayerDataModel playerDataModel, ColorScheme colorScheme)
        {
            ResetScore();
            PlayerLevelStatsData stats = playerDataModel.playerData.GetPlayerLevelStatsData(beatmap);
            Highscore = stats.highScore;
            float startTime = setupData.practiceSettings.startSongTime;
            // This LINQ statement is to ensure compatibility with Practice Mode / Practice Plugin
            int notesLeft = noteCountProcessor.Data.Count(x => x.time > startTime);
            MaxScoreAtLevelStart = ScoreHelpers.CalculateMaxScore(notesLeft);
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


        private int CalculateScoreFromCurrentPercentage() {
            if (MaxScoreTotal == 0) {
                return 0;
            }
            double currentRatio = ScoreHelpers.CalculateRatio(ScoreTotal, MaxScoreTotal);
            return (int)Math.Round(currentRatio * MaxScoreAtLevelStart);
        }


        private void LogScoreChange()
        {
            Plugin.Log.Debug("VisualScoreCounter : ScoreTotal: " + ScoreTotal + ", MaxScoreTotal: " + MaxScoreTotal + ", PercentageTotal: " + PercentageTotal);
        }


        internal void AddScore(ColorType colorType, int score, int comboMultiplier, int fcMultiplier)
        {

            //Plugin.Log.Debug("VisualScoreCounter : AddScore | score: " + score + ", comboMultiplier: " + comboMultiplier + ", fcMultiplier: " + fcMultiplier);

            // Update score for left or right saber
            if (colorType == ColorType.ColorA) {

                double preScoreA = ScoreA;
                double preMaxScoreA = MaxScoreA;

                ScoreA += score * comboMultiplier;
                MaxScoreA += ScoreModel.GetNoteScoreDefinition(NoteData.ScoringType.Normal).maxCutScore * fcMultiplier;

            } else if (colorType == ColorType.ColorB) {

                double preScoreB = ScoreB;
                double preMaxScoreB = MaxScoreB;

                ScoreB += score * comboMultiplier;
                MaxScoreB += ScoreModel.GetNoteScoreDefinition(NoteData.ScoringType.Normal).maxCutScore * fcMultiplier;

            }

            // Inform listeners that the score has updated
            InvokeScoreUpdate();
        }

        internal void SubtractScore(ColorType colorType, int score, int comboMultiplier, int fcMultiplier, bool subtractFromMaxScore = false)
        {
            //Plugin.Log.Debug("VisualScoreCounter : SubtractScore | score: " + score + ", comboMultiplier: " + comboMultiplier + ", fcMultiplier: " + fcMultiplier);
            // Update score for left or right saber
            if (colorType == ColorType.ColorA)
            {
                ScoreA -= score * comboMultiplier;
                if (subtractFromMaxScore) MaxScoreA -= ScoreModel.GetNoteScoreDefinition(NoteData.ScoringType.Normal).maxCutScore * fcMultiplier;
            }
            else if (colorType == ColorType.ColorB)
            {
                ScoreB -= score * comboMultiplier;
                if (subtractFromMaxScore) MaxScoreB -= ScoreModel.GetNoteScoreDefinition(NoteData.ScoringType.Normal).maxCutScore * fcMultiplier;
            }

            // Inform listeners that the score has updated
            InvokeScoreUpdate();
        }

        public void syncScore(int realScore, int realMaxScore)
        {
            /*
            ScoreA = Convert.ToInt32(Math.Round(realScore / 2.0d));
            ScoreB = Convert.ToInt32(Math.Round(realScore / 2.0d));
            MaxScoreA = Convert.ToInt32(Math.Round(realMaxScore / 2.0d));
            MaxScoreB = Convert.ToInt32(Math.Round(realMaxScore / 2.0d));
            InvokeScoreUpdate();
            */
        }





    }

}
