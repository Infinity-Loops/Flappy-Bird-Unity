using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DotGears.Flappy
{
    public class Pipe : MonoBehaviour
    {
        public static Pipe Instance = null;
        public GameObject Pipes;
        public GameObject SpecialPipes;
        public float PipesSpeed;
        public float InitialPosition;
        public float NextPosition;
        public float MaxYPosition;
        public float MinYPosition;
        public GameObject CurrentPipe;
        public GameObject OldPipe;
        public bool UseSpecial;
        private void Awake()
        {
            Instance = this;
        }
        public void RenderInitialPipe()
        {
            if (Bird.Instance.CanMove)
            {
                CurrentPipe = Instantiate(Pipes);
                var pos = CurrentPipe.transform.position;
                pos.y = Random.Range(MinYPosition, MaxYPosition);
                pos.x = InitialPosition;
                CurrentPipe.transform.position = pos;
            }
        }
        public void CreateNextPipe()
        {
            if (Bird.Instance.CanMove)
            {
                OldPipe = CurrentPipe;
                if (UseSpecial)
                {
                    CurrentPipe = Instantiate(SpecialPipes);
                }
                else
                {
                    CurrentPipe = Instantiate(Pipes);
                }
                var pos = CurrentPipe.transform.position;
                pos.y = Random.Range(MinYPosition, MaxYPosition);
                pos.x = OldPipe.transform.position.x + NextPosition;
                CurrentPipe.transform.position = pos;
            }
        }
    }
}