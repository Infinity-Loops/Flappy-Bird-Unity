using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DotGears.Flappy
{
    // MAKE MOVING PLATFORM EFFECT
    public class Land : MonoBehaviour
    {
        public static Land Instance = null;
        public float StartLandWidth;
        public float LandWidthMultiplier;
        public SpriteRenderer Renderer => GetComponent<SpriteRenderer>();
        private void Awake()
        {
            Instance = this;
        }
        void Update()
        {
            var Size = Renderer.size;
            Size.x += (LandWidthMultiplier) * Time.smoothDeltaTime;
            Renderer.size = (Size);
        }
    }
}
