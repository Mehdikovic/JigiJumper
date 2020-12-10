using JigiJumper.Data;
using JigiJumper.Managers;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;


namespace JigiJumper.UI
{
    public class GameWindowUi : WindowUi
    {
        [Header("Windows")]
        [SerializeField] private WindowUi _homeWindow = null;

        [Header("Game Mode Window Buttons")]
        [SerializeField] private Button _btnEasy = null;
        [SerializeField] private Button _btnNormal = null;
        [SerializeField] private Button _btnHard = null;
        [SerializeField] private Button _btnBack = null;

        private void Awake()
        {
            InitialComponent();

            SetActivation(false, _behaviorUIs);
            _selfRect.gameObject.SetActive(false);
        }

        private void InitialComponent()
        {
            _behaviorUIs = new Behaviour[]
            {
                _btnEasy,
                _btnNormal,
                _btnHard,
                _btnBack,
            };

            _btnEasy.onClick.AddListener(() => LoadTheGame(LevelType.Easy));
            _btnNormal.onClick.AddListener(() => LoadTheGame(LevelType.Normal));
            _btnHard.onClick.AddListener(() => LoadTheGame(LevelType.Hard));
            _btnBack.onClick.AddListener(() => TransitionToWindow(this, _homeWindow));

        }

        private void LoadTheGame(LevelType type)
        {
            SetActivation(false, _behaviorUIs);
            _selfRect.gameObject.SetActive(false);

            _setting.levelType = type;
            if (!_setting.GetShowBannerOption())
                Advertisement.Banner.Hide();

            FindObjectOfType<SceneManagement>().LoadSceneAsyncAfter(0, 2);
        }
    }
}