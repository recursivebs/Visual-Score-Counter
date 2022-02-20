using System;
using System.Collections.Generic;
using Zenject;
using VisualScoreCounter.VSCounter.Configuration;
using CountersPlus.Counters.Interfaces;
using TMPro;
using VisualScoreCounter.Utils;

namespace VisualScoreCounter.Core
{
	public class ScoreTracker : IInitializable, IDisposable, ISaberSwingRatingCounterDidFinishReceiver, IScoreEventHandler
	{
		private int MaxScoreIfFinished(int acc) => acc + ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore;

		[InjectOptional] private GameplayCoreSceneSetupData sceneSetupData = null!;
		[Inject] private PlayerDataModel playerDataModel = null!;
		[Inject] private ComboUIController comboUIController = null!;
		[Inject] private CoreGameHUDController _gameHUDController = null!;

		private readonly ScoreController scoreController;
		private readonly ScoreManager scoreManager;

		[Inject] private readonly RelativeScoreAndImmediateRankCounter relativeScoreAndImmediateRank;

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

		public int GetMultiplierForCombo(int c)
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

		public void Initialize() {

			// Do not initialize if either of these are null
			if (playerDataModel == null || sceneSetupData == null)
				return;

			// Reset ScoreManager at level start
			scoreManager.ResetScoreManager(sceneSetupData.difficultyBeatmap, playerDataModel, sceneSetupData.colorScheme);

			// Set function for multiplier.
			// TODO: Move this into a setting?
			GetMultiplier = MultiplierAtNoteCount;

			// Assign events
			if (scoreController != null) {
				scoreController.noteWasMissedEvent += ScoreController_noteWasMissedEvent;
				scoreController.noteWasCutEvent += ScoreController_noteWasCutEvent;
			}

			ScoreControllerWallCollisionDetectionPatch.wallCollisionEvent += WallCollisionDetector_wallCollisionEvent;

		}

		private void WallCollisionDetector_wallCollisionEvent() {
			OnBreakCombo();
		}


		public void Dispose()
		{
			// Unassign events
			if (scoreController != null) {
				scoreController.noteWasMissedEvent -= ScoreController_noteWasMissedEvent;
				scoreController.noteWasCutEvent -= ScoreController_noteWasCutEvent;
			}
			ScoreControllerWallCollisionDetectionPatch.wallCollisionEvent -= WallCollisionDetector_wallCollisionEvent;
		}

		private void OnBreakCombo()
        {
			combo = 0;
            liveMultiplierCombo = 0;
			if (liveMultiplier == 8) {
				liveMultiplier = 4;
			} else if (liveMultiplier == 4) {
				liveMultiplier = 2;
			} else {
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
			if (noteData.colorType == ColorType.None) {
				return;
            }

			noteCount++;
			OnMiss(noteData);
		}

		private void ScoreController_noteWasCutEvent(NoteData noteData, in NoteCutInfo noteCutInfo, int _)
		{
			// Ignore bombs
			if (noteData.colorType == ColorType.None) {
				// We hit a bomb!
				OnBreakCombo();
				return;
            }

			if (!noteCutInfo.allIsOK)
            {
				// We have a bad cut!
				noteCount++;
				OnMiss(noteData);
				return;
            }

			// And ignore bad cuts. But do count them for proper application of the multiplier
			if (noteCutInfo.allIsOK) {

                noteCount++;
				OnGainCombo();

				// Track cut data
				swingCounterCutData.Add(noteCutInfo.swingRatingCounter, new CutData(noteData.colorType, noteCutInfo.cutDistanceToCenter, noteCount, combo));
				noteCutInfo.swingRatingCounter.RegisterDidFinishReceiver(this);

				// Add provisional score assuming it'll be a full swing to make it feel more responsive even though it may be temporarily incorrect
				ScoreModel.RawScoreWithoutMultiplier(noteCutInfo.swingRatingCounter, noteCutInfo.cutDistanceToCenter, out _, out _, out int acc);
				scoreManager.AddScore(noteData.colorType, MaxScoreIfFinished(acc), liveMultiplier, GetMultiplier(noteCount));

			}

		}

		public void HandleSaberSwingRatingCounterDidFinish(ISaberSwingRatingCounter saberSwingRatingCounter)
		{
			if (swingCounterCutData.TryGetValue(saberSwingRatingCounter, out CutData cutData)) {
				// Calculate difference between previously applied score and actual score
				int diffAngleCutScore = DifferenceFromProvisionalScore(saberSwingRatingCounter, cutData.cutDistanceToCenter);

				// If the previously applied score was NOT correct (aka, it was a full NOT swing) -> Update score
				if (diffAngleCutScore > 0) {
					scoreManager.SubtractScore(cutData.colorType, diffAngleCutScore, liveMultiplier, GetMultiplier(cutData.noteCount));
				}

				// Remove cut data since it won't be needed again.
				swingCounterCutData.Remove(saberSwingRatingCounter);
			} else {
				//Plugin.Log.Error("ScoreTracker, HandleSaberSwingRatingCounterDidFinish : Failed to get cutData from swingCounterCutData!");
            }

			// Unregister saber swing rating counter.
			saberSwingRatingCounter.UnregisterDidFinishReceiver(this);
		}

		private int DifferenceFromProvisionalScore(ISaberSwingRatingCounter saberSwingRatingCounter, float cutDistanceToCenter)
		{
			// note: Accuracy won't change over time, therefore it can be ignored in the calculation since it'll just cancel out.
			ScoreModel.RawScoreWithoutMultiplier(saberSwingRatingCounter, cutDistanceToCenter, out int preCut, out int postCut, out _);

			int maxAngleCutScore = ScoreModel.kMaxBeforeCutSwingRawScore + ScoreModel.kMaxAfterCutSwingRawScore;
			int ratingAngleCutScore = preCut + postCut;

			return maxAngleCutScore - ratingAngleCutScore;
		}

		public bool IsAtEndOfSong() {
			return (int)lastBaseGameMaxScoreUpdated == scoreManager.MaxScoreAtLevelStart;
        }

        public void ScoreUpdated(int modifiedScore) {
			lastBaseGameScoreUpdated = modifiedScore;
			if (IsAtEndOfSong() || (ScoresaberUtil.IsInReplay() && IsScoreTooDifferent(modifiedScore))) {
                scoreManager.syncScore((int) lastBaseGameScoreUpdated, (int) lastBaseGameMaxScoreUpdated);
            }
        }

        public void MaxScoreUpdated(int maxModifiedScore) {
			lastBaseGameMaxScoreUpdated = maxModifiedScore;
			if (IsAtEndOfSong()) {
				scoreManager.syncScore((int) lastBaseGameScoreUpdated, (int) lastBaseGameMaxScoreUpdated);
            }
        }


		private bool IsScoreTooDifferent(int scoreUpdatedModifiedScore) {
            int scoreDiff = Convert.ToInt32(Math.Abs(scoreUpdatedModifiedScore - scoreManager.ScoreTotal));
            if (scoreDiff > 250) {
				return true;
            }
			return false;
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

