using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace DotGears.Flappy
{
    public class ScoreAnimator : MonoBehaviour
    {
        public float ScrollDuration;
        public CanvasGroup GameOver;
        private void Start()
        {
            (transform as RectTransform).DOAnchorPosY(18, ScrollDuration);
            GameOver.DOFade(1, 0.2f);
        }
    }
}