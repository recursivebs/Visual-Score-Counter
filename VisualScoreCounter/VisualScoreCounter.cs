using CountersPlus.ConfigModels;
using CountersPlus.Utils;
using CountersPlus.Counters.Custom;
using CountersPlus.Counters.Interfaces;
using TMPro;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using IPA.Utilities;
using System;
using HMUI;
using BeatSaberMarkupLanguage;
using System.Linq;

namespace VisualScoreCounter
{
    internal class VisualScoreCounter : BasicCustomCounter, IScoreEventHandler
    {

        [Inject] private RelativeScoreAndImmediateRankCounter relativeScoreAndImmediateRank;

        // Ring vars
        private readonly string multiplierImageSpriteName = "Circle";
        private readonly Vector3 ringSize = Vector3.one * 1.175f;
        private ImageView progressRing;

        private TMP_Text percentMajorText;
        private TMP_Text percentMinorText;

        public override void CounterInit()
        {
            percentMajorText = CanvasUtility.CreateTextFromSettings(Settings);
            percentMajorText.fontSize = 7;
            percentMinorText = CanvasUtility.CreateTextFromSettings(Settings);
            percentMinorText.fontSize = 3;

            HUDCanvas currentSettings = CanvasUtility.GetCanvasSettingsFromID(Settings.CanvasID);

            var canvas = CanvasUtility.GetCanvasFromID(Settings.CanvasID);
            if (canvas != null)
            {
                Vector2 ringAnchoredPos = CanvasUtility.GetAnchoredPositionFromConfig(Settings) * currentSettings.PositionScale;

                ImageView backgroundImage = CreateRing(canvas);
                backgroundImage.rectTransform.anchoredPosition = ringAnchoredPos;
                backgroundImage.CrossFadeAlpha(0.05f, 1f, false);
                backgroundImage.transform.localScale = ringSize / 10;
                backgroundImage.type = Image.Type.Simple;

                progressRing = CreateRing(canvas);
                progressRing.rectTransform.anchoredPosition = ringAnchoredPos;
                progressRing.transform.localScale = ringSize / 10;

            }
            UpdateRing();

            percentMajorText.rectTransform.anchoredPosition += new Vector2(0.0f, 0.7f);
            percentMinorText.rectTransform.anchoredPosition += new Vector2(0.0f, -3.0f);

            UpdateScoreText();

        }

        public override void CounterDestroy()
        {
        }

        private float GetCurrentPercentage()
        {
            float relativeScore = relativeScoreAndImmediateRank.relativeScore * 100;
            if (relativeScore <= 0)
            {
                relativeScore = 100.0f;
            }
            return relativeScore;
        }

        private int GetCurrentMajorPercentage()
        {
            float relativeScore = GetCurrentPercentage();
            int majorPercent = (int)Math.Floor(relativeScore);
            return majorPercent;
        }

        private int GetCurrentMinorPercentage()
        {
            float relativeScore = GetCurrentPercentage();
            int minorPercent = (int)((relativeScore % 1) * 100);
            return minorPercent;
        }


        private void UpdateScoreText()
        {
            float relativeScore = GetCurrentPercentage();
            int majorPercent = GetCurrentMajorPercentage();
            int minorPercent = GetCurrentMinorPercentage();
            Color percentMajorColor = GetColorForScore(relativeScore);
            Color percentMinorColor = percentMajorColor;
            if (Configuration.Instance.percentageRingShowsNextRankColor)
            {
                percentMinorColor = GetColorForScore(relativeScore + 1);
            }
            percentMajorText.text = string.Format("{0:D2}", majorPercent);
            percentMajorText.color = percentMajorColor;
            percentMinorText.text = string.Format("{0:D2}", minorPercent);
            percentMinorText.color = percentMinorColor;
        }


        private Color GetColorForScore(float Score) {

            // 100%
            if (Score >= 100.0f)
            {
                return Configuration.Instance.Color_100;
            }

            // 99%
            if (Score >= 99.0f && Score < 100.0f)
            {
                return Configuration.Instance.Color_99;
            }

            // 98%
            if (Score >= 98.0f && Score < 99.0f)
            {
                return Configuration.Instance.Color_98;
            }

            // 97%
            if (Score >= 97.0f && Score < 98.0f)
            {
                return Configuration.Instance.Color_97;
            }

            // 96%
            if (Score >= 96.0f && Score < 97.0f)
            {
                return Configuration.Instance.Color_96;
            }

            // 95%
            if (Score >= 95.0f && Score < 96.0f)
            {
                return Configuration.Instance.Color_95;
            }

            // 94%
            if (Score >= 94.0f && Score < 95.0f)
            {
                return Configuration.Instance.Color_94;
            }

            // 93%
            if (Score >= 93.0f && Score < 94.0f)
            {
                return Configuration.Instance.Color_93;
            }

            // 92%
            if (Score >= 92.0f && Score < 93.0f)
            {
                return Configuration.Instance.Color_92;
            }

            // 91%
            if (Score >= 91.0f && Score < 92.0f)
            {
                return Configuration.Instance.Color_91;
            }

            // 90%
            if (Score >= 90.0f && Score < 91.0f)
            {
                return Configuration.Instance.Color_90;
            }

            // 89%
            if (Score >= 89.0f && Score < 90.0f)
            {
                return Configuration.Instance.Color_89;
            }

            // 88%
            if (Score >= 88.0f && Score < 89.0f)
            {
                return Configuration.Instance.Color_88;
            }

            // 80%
            if (Score >= 80.0f && Score < 88.0f)
            {
                return Configuration.Instance.Color_80;
            }

            // 65%
            if (Score >= 65.0f && Score < 80.0f)
            {
                return Configuration.Instance.Color_65;
            }

            // 50%
            if (Score >= 50.0f && Score < 65.0f)
            {
                return Configuration.Instance.Color_50;
            }

            // 35%
            if (Score >= 35.0f && Score < 50.0f)
            {
                return Configuration.Instance.Color_35;
            }

            // 20%
            if (Score >= 20.0f && Score < 35.0f)
            {
                return Configuration.Instance.Color_20;
            }

            // 0%
            if (Score >= 0.0f && Score < 20.0f)
            {
                return Configuration.Instance.Color_0;
            }

            return Color.white;

        }

        public void UpdateRing()
        {
            float relativeScore = GetCurrentPercentage();
            Color nextColor;
            if (Configuration.Instance.percentageRingShowsNextRankColor)
            {
                nextColor = GetColorForScore(relativeScore + 1);
            } else
            {
                nextColor = GetColorForScore(relativeScore);
            }
            if (progressRing)
            {
                progressRing.color = nextColor;
            }
            float ringFillAmount = relativeScore % 1;
            progressRing.fillAmount = ringFillAmount;
            progressRing.SetVerticesDirty();
        }

        private ImageView CreateRing(Canvas canvas)
        {
            // Unfortunately, there is no guarantee that I have the CoreGameHUDController, since No Text and Huds
            // completely disables it from spawning. So, to be safe, we recreate this all from scratch.
            GameObject imageGameObject = new GameObject("Ring Image", typeof(RectTransform));
            imageGameObject.transform.SetParent(canvas.transform, false);
            ImageView newImage = imageGameObject.AddComponent<ImageView>();
            newImage.enabled = false;
            newImage.material = Utilities.ImageResources.NoGlowMat;
            newImage.sprite = Resources.FindObjectsOfTypeAll<Sprite>().FirstOrDefault(x => x.name == multiplierImageSpriteName);
            newImage.type = Image.Type.Filled;
            newImage.fillClockwise = true;
            newImage.fillOrigin = 2;
            newImage.fillAmount = 1;
            newImage.fillMethod = Image.FillMethod.Radial360;
            newImage.enabled = true;
            return newImage;
        }

        private bool ShouldUpdate()
        {
            float RelativeScore = GetCurrentPercentage();
            if (RelativeScore > 100.0f)
            {
                return false;
            }
            return true;
        } 

        private bool TryUpdate()
        {
            if (!ShouldUpdate())
            {
                // TODO: Handle updating on next frame?
                return false;
            }
            UpdateScoreText();
            UpdateRing();
            return true;
        }

        public void ScoreUpdated(int modifiedScore)
        {
            TryUpdate();
        }

        public void MaxScoreUpdated(int maxModifiedScore) { }

    }
}
