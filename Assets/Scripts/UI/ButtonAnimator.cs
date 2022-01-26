using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace DotGears.Flappy
{
    public class ButtonAnimator : MonoBehaviour
    {
        public float Duration;
        public float Scale;
        public void StartAnimate()
        {
            StartCoroutine(Animate());
        }
        public IEnumerator Animate()
        {
            (transform as RectTransform).DOScale(Scale, Duration);
            yield return new WaitForSeconds(Duration);
            (transform as RectTransform).DOScale(1, Duration);
        }
    }
}