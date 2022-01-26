using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedalEffect : MonoBehaviour
{
    [Header("Effects")]
    public Sprite Effect0;
    public Sprite Effect1;
    public Sprite Effect2;
    public UnityEngine.UI.Image Renderer;
    public RectTransform Rect;
    public float AnimationTime;
    public float RandomTime;
    private void Start()
    {
        Rect = GetComponent<Transform>() as RectTransform;
        Renderer = GetComponent<UnityEngine.UI.Image>();
        StartCoroutine(Animate());
        StartCoroutine(RandomPositioner());
    }
    IEnumerator RandomPositioner()
    {
        while (true)
        {
            yield return new WaitForSeconds(RandomTime);
            var RandomPosition = new Vector2(Random.Range(Rect.rect.xMin, Rect.rect.xMax), Random.Range(Rect.rect.yMin, Rect.rect.yMax)) + Rect.anchoredPosition;
            Rect.anchoredPosition = RandomPosition;
        }
    }
    IEnumerator Animate()
    {
        while (true)
        {
            yield return new WaitForSeconds(AnimationTime / 100);
            Renderer.sprite = Effect0;
            yield return new WaitForSeconds(AnimationTime / 100);
            Renderer.sprite = Effect1;
            yield return new WaitForSeconds(AnimationTime / 100);
            Renderer.sprite = Effect2;
        }
    }
}
