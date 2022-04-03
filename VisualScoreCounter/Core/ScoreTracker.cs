using System;
using System.Collections.Generic;
using Zenject;
using VisualScoreCounter.VSCounter.Configuration;
using CountersPlus.Counters.Interfaces;
using CountersPlus.Counters.NoteCountProcessors;
using TMPro;
using VisualScoreCounter.Utils;
using System.Linq;

namespace VisualScoreCounter.Core
{
    public class ScoreTracker : IInitializable, IDisposable, IScoreEventHandler, INoteEventHandler
    {
        private int MaxScoreIfFinished(int acc) => acc + ScoreModel.GetNoteScoreDefinition(NoteData.ScoringType.Normal).maxBeforeCutScore + ScoreModel.GetNoteScoreDefinition(NoteData.ScoringType.Normal).maxAfterCutScore;

        [InjectOptional] private GameplayCoreSceneSetupData sceneSetupData = null!;
        [Inject] private NoteCountProcessor noteCountProcessor = null!;
        [Inject] private PlayerHeadAndObstacleInteraction obstacleInteraction = null!;
        [Inject] private PlayerDataModel playerDataModel = null!;

        private readonly ScoreController scoreController;
        private readonly ScoreManager scoreManager;

        private Dictionary<ISaberSwingRatingCounter, CutData> swingCounterCutData;

        private static readonly Func<int, int> MultiplierAtNoteCount = noteCount => noteCount > 13 ? OptimiseGetMultiplier() : (noteCount > 5 ? 4 : (noteCount > 1 ? 2 : 1));
        private static readonly Func<int, int> MultiplierAtMax = noteCount => 8;
        private static Func<int, int> GetMultiplier;
        private static int OptimiseGetMultiplier() { GetMultiplier = MultiplierAtMax; return 8; }

        //private readonly int badCutThreshold;
        private int noteCount;

        private int combo;
        private int liveMultiplier;
        private int liveMultiplierCombo;

        private float lastBaseGameScoreUpdated;
        private float lastBaseGameMaxScoreUpdated;


        private int notesLeft = 0;

        public ScoreTracker([InjectOptional] ScoreController scoreController, ScoreManager scoreManager)
        {
            this.scoreManager = scoreManager;
            this.scoreController = scoreController;

            swingCounterCutData = new Dictionary<ISaberSwingRatingCounter, CutData>();
            noteCount = 0;
            liveMultiplier = 1;
            liveMultiplierCombo = 0;

            GetMultiplier = x => 1;
            lastBaseGameScoreUpdated = 0;
            lastBaseGameMaxScoreUpdated = 0;

        }

        public void Initialize()
        {

            // Do not initialize if either of these are null
            if (playerDataModel == null || sceneSetupData == null)
                return;

            // Reset ScoreManager at level start
            scoreManager.ResetScoreManager(sceneSetupData.difficultyBeatmap, playerDataModel, sceneSetupData.colorScheme);

            // Set function for multiplier.
            // TODO: Move this into a setting?
            GetMultiplier = MultiplierAtNoteCount;

            // Assign events
            if (scoreController != null)
            {
                scoreController.scoringForNoteFinishedEvent += ScoreController_scoringForNoteFinishedEvent;

            }

            obstacleInteraction.headDidEnterObstaclesEvent += WallCollisionDetector_wallCollisionEvent;

            // Initialize Note Counter
            if (sceneSetupData.practiceSettings != null && sceneSetupData.practiceSettings.startInAdvanceAndClearNotes)
            {
                float startTime = sceneSetupData.practiceSettings.startSongTime;
                // This LINQ statement is to ensure compatibility with Practice Mode / Practice Plugin
                notesLeft = noteCountProcessor.Data.Count(x => x.time > startTime);
            }
            else
            {
                notesLeft = noteCountProcessor.NoteCount;
            }

        }

        private void WallCollisionDetector_wallCollisionEvent()
        {
            OnBreakCombo();
        }


        public void Dispose()
        {
            // Unassign events
            if (scoreController != null) {
                scoreController.scoringForNoteFinishedEvent -= ScoreController_scoringForNoteFinishedEvent;
            }
            obstacleInteraction.headDidEnterObstaclesEvent -= WallCollisionDetector_wallCollisionEvent;
        }

        private void OnBreakCombo()
        {
            combo = 0;
            liveMultiplierCombo = 0;
            if (liveMultiplier == 8)
            {
                liveMultiplier = 4;
            }
            else if (liveMultiplier == 4)
            {
                liveMultiplier = 2;
            }
            else
            {
                liveMultiplier = 1;
            }
        }

        private void OnGainCombo()
        {
            combo++;
            liveMultiplierCombo++;
            if (liveMultiplier == 4 && liveMultiplierCombo > 7)
            {
                liveMultiplier = 8;
                liveMultiplierCombo = 0;
                return;
            }
            if (liveMultiplier == 2 && liveMultiplierCombo > 3)
            {
                liveMultiplier = 4;
                liveMultiplierCombo = 0;
                return;
            }
            if (liveMultiplier == 1 && liveMultiplierCombo > 1)
            {
                liveMultiplier = 2;
                liveMultiplierCombo = 0;
                return;
            }
        }

        private void OnMiss(NoteData noteData)
        {
            int mult = liveMultiplier;
            int fcMult = GetMultiplier(noteCount);
            OnBreakCombo();
            scoreManager.AddScore(noteData.colorType, 0, 1, fcMult);
        }

        private void ScoreController_noteWasMissedEvent(NoteData noteData, int _)
        {
            if (noteData.colorType == ColorType.None)
            {
                return;
            }

            noteCount++;
            OnMiss(noteData);
        }


        private void ScoreController_scoringForNoteFinishedEvent(ScoringElement scoringElement) {


            if (scoringElement.noteData.colorType == ColorType.None) {
                // We hit a bomb!
                OnBreakCombo();
                return;
            }


            if (scoringElement is MissScoringElement miss) {
                int mult = liveMultiplier;
                int fcMult = GetMultiplier(noteCount);
                OnBreakCombo();
                scoreManager.AddScore(scoringElement.noteData.colorType, 0, 1, fcMult);
            }


            if (scoringElement is BadCutScoringElement badCut) {
                // We have a bad cut!
                noteCount++;
                OnMiss(scoringElement.noteData);
                return;
            }


            if (scoringElement.noteData.scoringType != NoteData.ScoringType.Normal) {
                // No support for new notes yet.
                return;
            }

            scoreManager.AddScore(scoringElement.noteData.colorType, scoringElement.cutScore, liveMultiplier, GetMultiplier(noteCount));

        }


        public bool IsAtEndOfSong()
        {
            return notesLeft <= 0;
        }

        public void ScoreUpdated(int modifiedScore)
        {
            lastBaseGameScoreUpdated = modifiedScore;
            if (IsAtEndOfSong() || (ScoresaberUtil.IsInReplay() && IsScoreTooDifferent(modifiedScore)))
            {
                scoreManager.syncScore((int)lastBaseGameScoreUpdated, (int)lastBaseGameMaxScoreUpdated);
            }
        }

        public void MaxScoreUpdated(int maxModifiedScore)
        {
            lastBaseGameMaxScoreUpdated = maxModifiedScore;
            if (IsAtEndOfSong())
            {
                scoreManager.syncScore((int)lastBaseGameScoreUpdated, (int)lastBaseGameMaxScoreUpdated);
            }
        }


        private bool IsScoreTooDifferent(int scoreUpdatedModifiedScore)
        {
            int scoreDiff = Convert.ToInt32(Math.Abs(scoreUpdatedModifiedScore - scoreManager.ScoreTotal));
            if (scoreDiff > 250)
            {
                return true;
            }
            return false;
        }

        public void OnNoteCut(NoteData data, NoteCutInfo info)
        {
            if (data.colorType != ColorType.None && !noteCountProcessor.ShouldIgnoreNote(data))
            {
                DecrementCounter();
            }
        }

        public void OnNoteMiss(NoteData data)
        {
            if (data.colorType != ColorType.None && !noteCountProcessor.ShouldIgnoreNote(data))
            {
                DecrementCounter();
            }
        }

        private void DecrementCounter()
        {
            --notesLeft;
            if (IsAtEndOfSong())
            {
                scoreManager.syncScore((int)lastBaseGameScoreUpdated, (int)lastBaseGameMaxScoreUpdated);
            }
        }

        private struct CutData
        {
            public ColorType colorType { get; private set; }
            public float cutDistanceToCenter { get; private set; }
            public int noteCount { get; private set; }
            public int combo { get; private set; }

            public CutData(ColorType colorType, float cutDistanceToCenter, int noteCount, int combo)
            {
                this.colorType = colorType;
                this.cutDistanceToCenter = cutDistanceToCenter;
                this.noteCount = noteCount;
                this.combo = combo;
            }
        }

    }
}

