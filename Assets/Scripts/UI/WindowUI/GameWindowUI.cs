using JigiJumper.Data;
using JigiJumper.Managers;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;


namespace JigiJumper.Ui {
    public class GameWindowUi : WindowUi {
        [Header("Windows")]
        [SerializeField] private WindowUi _homeWindow = null;

        [Header("Game Mode Window Buttons")]
        [SerializeField] private Button _btnEasy = null;
        [SerializeField] private Button _btnNormal = null;
        [SerializeField] private Button _btnHard = null;
        [SerializeField] private Button _btnBack = null;

        protected override void OnAwake() {
            InitialComponent();

            SetBehaviorActivation(false);
            _selfRectWindow.gameObject.SetActive(false);
        }

        protected override Behaviour[] Behaviors() {
            return new Behaviour[] {
                _btnEasy,
                _btnNormal,
                _btnHard,
                _btnBack,
            };
        }

        private void InitialComponent() {
            _btnEasy.onClick.AddListener(() => LoadTheGame(LevelType.Easy));
            _btnNormal.onClick.AddListener(() => LoadTheGame(LevelType.Normal));
            _btnHard.onClick.AddListener(() => LoadTheGame(LevelType.Hard));
            _btnBack.onClick.AddListener(() => Ui.TransitionTo(this, _homeWindow));
        }

        private void LoadTheGame(LevelType type) {
            SetBehaviorActivation(false);
            _selfRectWindow.gameObject.SetActive(false);

            _setting.levelType = type;
            if (!_setting.GetShowBannerOption())
                Advertisement.Banner.Hide();

            FindObjectOfType<SceneManagement>().LoadSceneAsyncAfter(0, sceneIndex: 2);
        }
    }
}