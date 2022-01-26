using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DotGears.Flappy
{
    public class PipeMover : MonoBehaviour
    {
        public float Speed;
        bool nextpipe = false;
        Renderer SpriteRenderer;
        Collider2D[] Colliders;
        private void Awake()
        {
            SpriteRenderer = transform.GetChild(0).GetComponent<Renderer>();
            Colliders = transform.GetComponentsInChildren<Collider2D>();
        }
        void Update()
        {
            if (Bird.Instance.CanMove)
            {
                Vector3 pos = transform.position;
                pos.x -= (Speed) * Time.smoothDeltaTime;
                transform.position = pos;
                if (SpriteRenderer.isVisible && !nextpipe)
                {
                    Pipe.Instance.CreateNextPipe();
                    nextpipe = true;
                }
                if(nextpipe && !SpriteRenderer.isVisible)
                {
                    Destroy(gameObject);
                }
            } else
            {
                for (int i = 0; i < Colliders.Length; i++)
                {
                    if (Colliders[i].isTrigger)
                    {
                        continue;
                    }
                    Colliders[i].enabled = false;
                }
            }
        }
    }
}