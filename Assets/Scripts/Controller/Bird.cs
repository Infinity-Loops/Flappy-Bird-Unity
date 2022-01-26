using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace DotGears.Flappy
{
    public class Bird : MonoBehaviour
    {
        public static Bird Instance = null;
        [Header("Yellow Bird Sprites")]
        public Sprite YBirdAnim0;
        public Sprite YBirdAnim1;
        public Sprite YBirdAnim2;
        [Header("Red Bird Sprites")]
        public Sprite RBirdAnim0;
        public Sprite RBirdAnim1;
        public Sprite RBirdAnim2;
        [Header("Blue Bird Sprites")]
        public Sprite BBirdAnim0;
        public Sprite BBirdAnim1;
        public Sprite BBirdAnim2;
        [Header("Settings")]
        public CurrentBird BirdType = CurrentBird.Yellow;
        public Transform CenterOfMass;
        public float Velocity = 2f;
        [Header("Game Movement")]
        public int MaxTapsPerTouch = 2;
        int CurrentTapsPerTouch;
        public bool CanMove = false;
        public float CommonAnimSpeed = 10;
        public bool Tapping;
        [Header("Idle Movement")]
        public bool Animate = true;
        public bool Idle = true;
        public float IdleAnimSpeed = 10;
        public float IdleVerticalDuration = 1;
        public float IdleVerticalUp = 0.8f;
        public float IdleVerticalDown = 0.6f;
        [Header("Collision")]
        public float MaxYPosition;
        [Header("Physics")]
        public bool BirdCanRotate = true;
        public float BirdFallTransitionTime = 0.25f;
        public float BirdRotationMultiplier = 0.6f;
        public float MaxBirdRotation = 0.2f;
        [Header("Audio")]
        public AudioClip Die;
        public AudioClip Hit;
        public AudioClip Point;
        public AudioClip Swooshing;
        public AudioClip Wing;
        Rigidbody2D Rigidbody => GetComponent<Rigidbody2D>();
        SpriteRenderer Renderer => GetComponent<SpriteRenderer>();
        AudioSource AudioSource => GetComponent<AudioSource>();
        private void Awake()
        {
            Rigidbody.centerOfMass = CenterOfMass.position;
            switch (Random.Range(0, 3))
            {
                case 1:
                    BirdType = CurrentBird.Yellow;
                    break;
                case 2:
                    BirdType = CurrentBird.Red;
                    break;
                case 3:
                    BirdType = CurrentBird.Blue;
                    break;
            }
            if(Instance != null)
            {
                DOTween.Kill(transform);
            }
            Instance = this;
        }
        private void Start()
        {
            StartCoroutine(IdleAnimate());
            IdleUp();
        }
        public void IdleUp()
        {
            if (Idle)
            {
                Rigidbody.DOMoveY(IdleVerticalUp, IdleVerticalDuration);
                StartCoroutine(InvokeMethod(IdleDown, IdleVerticalDuration));
            }
        }
        public void IdleDown()
        {
            if (Idle)
            {
                Rigidbody.DOMoveY(IdleVerticalDown, IdleVerticalDuration);
                StartCoroutine(InvokeMethod(IdleUp, IdleVerticalDuration));
            }
        }
        delegate void Invokable();
        IEnumerator InvokeMethod(Invokable Method,float Time)
        {
            yield return new WaitForSeconds(Time);
            Method.Invoke();
        }
        private void Update()
        {
            Tapping = Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
            if (Idle)
            {
                Rigidbody.bodyType = RigidbodyType2D.Kinematic;
            }
            else
            {
                Rigidbody.bodyType = RigidbodyType2D.Dynamic;
            }
            if (Tapping && !Idle && CanMove && !(CurrentTapsPerTouch >= MaxTapsPerTouch))
            {
                CurrentTapsPerTouch++;
                AudioSource.PlayOneShot(Wing);
                Rigidbody.AddForce(Vector2.up * Velocity,ForceMode2D.Force);
            }
            else if (CurrentTapsPerTouch >= MaxTapsPerTouch)
            {
                CurrentTapsPerTouch = 0;
            }
            if (!Idle && BirdCanRotate)
            {
                float Rotation = Rigidbody.velocity.y * BirdRotationMultiplier;
                if (transform.rotation.z > MaxBirdRotation && Rotation > 0)
                {
                    Rotation = 0;
                }
                else if (transform.rotation.z < -MaxBirdRotation && Rotation < 0)
                {
                    Rotation = 0;
                }
                transform.Rotate(0, 0, Rotation, Space.Self);
            }
            ClampPosition();
        }
        public void ClampPosition()
        {
            var pos = Rigidbody.position;
            pos.y = Mathf.Clamp(pos.y, float.MinValue, MaxYPosition);
            Rigidbody.position = pos;
        }
        IEnumerator IdleAnimate()
        {
            while (Animate)
            {
                switch (BirdType)
                {
                    case CurrentBird.Yellow:
                        yield return new WaitForSeconds(IdleAnimSpeed / 100);
                        Renderer.sprite = YBirdAnim0;
                        yield return new WaitForSeconds(IdleAnimSpeed / 100);
                        Renderer.sprite = YBirdAnim1;
                        yield return new WaitForSeconds(IdleAnimSpeed / 100);
                        Renderer.sprite = YBirdAnim2;
                        break;
                    case CurrentBird.Red:
                        yield return new WaitForSeconds(IdleAnimSpeed / 100);
                        Renderer.sprite = RBirdAnim0;
                        yield return new WaitForSeconds(IdleAnimSpeed / 100);
                        Renderer.sprite = RBirdAnim1;
                        yield return new WaitForSeconds(IdleAnimSpeed / 100);
                        Renderer.sprite = RBirdAnim2;
                        break;
                    case CurrentBird.Blue:
                        yield return new WaitForSeconds(IdleAnimSpeed / 100);
                        Renderer.sprite = BBirdAnim0;
                        yield return new WaitForSeconds(IdleAnimSpeed / 100);
                        Renderer.sprite = BBirdAnim1;
                        yield return new WaitForSeconds(IdleAnimSpeed / 100);
                        Renderer.sprite = BBirdAnim2;
                        break;
                }
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision != null && BirdCanRotate)
            {
                Land.Instance.LandWidthMultiplier = 0;
                Animate = false;
                CanMove = false;
                transform.DORotateQuaternion(Quaternion.Euler(0, 0, -90), BirdFallTransitionTime);
                BirdCanRotate = false;
                StartCoroutine(DieSound());
            }
        }
        public void PlaySound(AudioClip Sound)
        {
            AudioSource.PlayOneShot(Sound);
        }
        IEnumerator DieSound()
        {
            AudioSource.PlayOneShot(Hit);
            yield return new WaitForSeconds(0.1f);
            AudioSource.PlayOneShot(Die);
            GameActivity.Instance.ShowScoreScreen();
        }
    }
    public enum CurrentBird
    {
        Yellow,
        Red,
        Blue
    }
}