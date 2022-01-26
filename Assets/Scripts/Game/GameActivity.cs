using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace DotGears.Flappy
{
    public class GameActivity : MonoBehaviour
    {
        public static GameActivity Instance = null;
        [Header("Game Variables")]
        public UnityEngine.UI.Image MedalPlace;
        public Sprite Medal0;
        public Sprite Medal1;
        public Sprite Medal2;
        public Sprite Medal3;
        public Sprite Medal4;
        public UnityEngine.UI.Text ScorePlace;
        public UnityEngine.UI.Text BestScorePlace;
        public UnityEngine.UI.Image ScoreDraw;
        public UnityEngine.UI.Image ScoreDraw2;
        public GameObject New;
        public Sprite[] ScoreText;
        public int Score;
        [Header("Environment")]
        public SpriteRenderer Background;
        public Sprite DayBackground;
        public Sprite NightBackground;
        [Header("Global Settings")]
        public float TextAnimation = 1f;
        public float FadeDuration = 0.5f;
        public float IdleVerticalUp = 0.5f;
        public float IdleVerticalDown = 0.4f;
        [Header("Game Screen Settings")]
        public UnityEngine.UI.Image SceneFade;
        public bool Tapping;
        public bool FadeGameScreen = false;
        public bool OnGameScreen;
        [Header("Game Screens")]
        public CanvasGroup HomeScreen;
        public CanvasGroup GameScreen;
        public CanvasGroup ScoreScreen;
        bool CloseGameScreenRequested;
        public void UpdateScore()
        {
            string scoretxt = $"{Score}";
            var numbers = scoretxt.ToCharArray();
            if (numbers.Length == 1)
            {
                int firstchar = int.Parse(numbers[0].ToString());
                ScoreDraw.sprite = ScoreText[firstchar];
            }
            else
            {
                ScoreDraw2.color = ScoreDraw2.color = new Color(1, 1, 1, 0);
            }
            if (numbers.Length == 2)
            {
                int secondchar = int.Parse(numbers[1].ToString());
                int firstchar = int.Parse(numbers[0].ToString());
                ScoreDraw2.sprite = ScoreText[firstchar];
                ScoreDraw.sprite = ScoreText[secondchar];
                ScoreDraw2.color = ScoreDraw2.color = new Color(1, 1, 1, 1);
            }

        }
        private void Awake()
        {
            switch (Random.Range(0, 3))
            {
                case 1:
                    Background.sprite = DayBackground;
                    break;
                case 2:
                    Background.sprite = NightBackground;
                    break;
            }
            Instance = this;
        }
        private void Update()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            Tapping = Touch.GetTouchDown();
#else
            Tapping = Input.GetMouseButtonDown(0);
#endif
            if (Tapping && OnGameScreen)
            {
                if (!FadeGameScreen)
                {
                    FadeGameScreen = true;
                    ScoreDraw.gameObject.SetActive(true);
                    ScoreDraw2.gameObject.SetActive(true);
                    ScoreDraw2.color = new Color(1, 1, 1, 0);
                    CloseGameScreen();
                    CloseGameScreenRequested = true;
                    Bird.Instance.Idle = false;
                    Bird.Instance.CanMove = true;
                    Pipe.Instance.RenderInitialPipe();
                }
            }
            if(Score > 49)
            {
                Pipe.Instance.UseSpecial = true;
            }
            else
            {
                Pipe.Instance.UseSpecial = false;
            }
        }
        public void CloseGameScreen()
        {
            if (!CloseGameScreenRequested)
            {
                GameScreen.DOFade(0, 0.15f);
            }
        }
        public void ResetGame()
        {
            SceneFade.DOFade(1, 0.05f);
            StartCoroutine(ResetCoroutine());
        }
        IEnumerator ResetCoroutine()
        {
            yield return new WaitForSeconds(0.05f);
            Bird.Instance.Idle = false;
            DOTween.KillAll();
            DOTween.Clear(true);
            DOTween.ClearCachedTweens();
            var dotween = FindObjectOfType<DG.Tweening.Core.DOTweenComponent>();
            DestroyImmediate(dotween);
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        IEnumerator SetTextIncremental(UnityEngine.UI.Text Text, int value, float time)
        {
            int svalue = 0;
            while (svalue != value)
            {
                for (int i = 0; i < value; i++)
                {
                    yield return new WaitForSeconds(time / 100);
                    svalue++;
                    Text.text = svalue.ToString();
                }
            }
        }
        public void ShowScoreScreen()
        {
            if (PlayerPrefs.HasKey("HighScore"))
            {
                if (Score > PlayerPrefs.GetInt("HighScore"))
                {
                    PlayerPrefs.SetInt("OldScore", PlayerPrefs.GetInt("HighScore"));
                    PlayerPrefs.SetInt("HighScore", Score);
                    New.SetActive(true);
                    StartCoroutine(SetTextIncremental(ScorePlace, Score, TextAnimation));
                    BestScorePlace.text = PlayerPrefs.GetInt("OldScore").ToString();
                    StartCoroutine(SetTextIncremental(BestScorePlace, Score, TextAnimation));
                }
                else if (Score < PlayerPrefs.GetInt("HighScore"))
                {
                    StartCoroutine(SetTextIncremental(ScorePlace, Score, TextAnimation));
                    StartCoroutine(SetTextIncremental(BestScorePlace, PlayerPrefs.GetInt("HighScore"), TextAnimation));
                }
            }
            else
            {
                if (Score != 0)
                {
                    New.SetActive(true);
                }
                PlayerPrefs.SetInt("HighScore", Score);
                StartCoroutine(SetTextIncremental(ScorePlace, Score, TextAnimation));
                StartCoroutine(SetTextIncremental(BestScorePlace, Score, TextAnimation));
            }
            if(Score >= 10)
            {
                MedalPlace.gameObject.SetActive(true);
                MedalPlace.sprite = Medal0;
            } else if(Score >= 20)
            {
                MedalPlace.gameObject.SetActive(true);
                MedalPlace.sprite = Medal1;
            } else if(Score >= 30)
            {
                MedalPlace.gameObject.SetActive(true);
                MedalPlace.sprite = Medal2;
            }
            else if (Score >= 40)
            {
                MedalPlace.gameObject.SetActive(true);
                MedalPlace.sprite = Medal3;
            }
            else if (Score >= 50)
            {
                MedalPlace.gameObject.SetActive(true);
                MedalPlace.sprite = Medal4;
            }
            ScoreDraw.gameObject.SetActive(false);
            ScoreDraw2.gameObject.SetActive(false);
            ScoreScreen.gameObject.SetActive(true);
            ScoreScreen.DOFade(1, FadeDuration);
        }
        public void ShowGameScreen()
        {
            OnGameScreen = true;
            Bird.Instance.transform.DOMoveX(-0.5f, 0.5f);
            Bird.Instance.transform.DOMoveY(0.4f, 0.5f);
            Bird.Instance.IdleVerticalDown = IdleVerticalDown;
            Bird.Instance.IdleVerticalUp = IdleVerticalUp;
            HomeScreen.DOFade(0, FadeDuration);
            Invoke("DisableHomeScreen", FadeDuration);
            SetActiveObject(GameScreen.gameObject, true);
            GameScreen.DOFade(1, FadeDuration);
        }
        public void DisableHomeScreen()
        {
            SetActiveObject(HomeScreen.gameObject, false);
        }
        public void SetActiveObject(GameObject Object, bool State)
        {
            Object.SetActive(State);
        }
    }
}