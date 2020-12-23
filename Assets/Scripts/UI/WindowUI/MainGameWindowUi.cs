using JigiJumper.Ads;
using JigiJumper.Managers;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace JigiJumper.Ui {
    public class MainGameWindowUi : WindowUi {
        [Header("Windows")]
        [SerializeField] private WindowUi _settingsWindow = null;
        [SerializeField] private PopupUi _popup = null;

        [Header("Reward-Ads")]
        [SerializeField] private RewardedAd _ads = null;

        [Header("Buttons")]
        [SerializeField] private Button _btnShowSettings = null;
        [SerializeField] private Button _btnShowAd = null;
        [SerializeField] private Button _btnHome = null;
        [SerializeField] private Button _btnRestart = null;
        [SerializeField] private Button _btnClose = null;

        [Header("GfxButtonsHolder")]
        [SerializeField] private GameObject _settingsItem = null;
        [SerializeField] private GameObject _watchAdItem = null;
        [SerializeField] private GameObject _closeItem = null;

        private int _remainingAds = 2;
        private GameManager _gameManager;
        private bool _isActivatingSettingWindow = false;

        protected override void OnAwake() {
            _gameManager = GameManager.instance;
            InitialUIComponents();
            _selfRectWindow.gameObject.SetActive(false);
            _gameManager.OnCompleteRestartRequest += OnCompleteRestartRequest;
            _gameManager.OnLevelChanged += OnLevelChanged;
        }


        protected override Behaviour[] Behaviors() {
            return new Behaviour[]
            {
                _btnShowAd,
                _btnHome,
                _btnRestart,
                _btnShowSettings,
                _btnClose,
            };
        }

        // called by button event subscriber at canvas
        public void ActivateSettingMenuWindow() {
            if (_isActivatingSettingWindow) { return; }

            _isActivatingSettingWindow = true;
            ActiveSettingsWindow(true);
            
            Ui.DoShowWindow(_selfRectWindow, () => _isActivatingSettingWindow = false);
        }

        private void InitialUIComponents() {
            _btnShowSettings.onClick.AddListener(() => {
                SetBehaviorActivation(false);
                Ui.DoShowWindow(_settingsWindow.GetRectWindow(),
                    () => {
                        Ui.SetBehaviorActivation(true, _settingsWindow.GetUiBehaviors());
                        SetBehaviorActivation(true);
                    }
                );
            });

            _btnRestart.onClick.AddListener(() => {
                SetBehaviorActivation(false);
                _gameManager.SaveRecords();
                FindObjectOfType<SceneManagement>().LoadSceneAsyncAfter(0, SceneManager.GetActiveScene().buildIndex);
            });

            _btnHome.onClick.AddListener(() => {
                SetBehaviorActivation(false);
                _gameManager.SaveRecords();
                FindObjectOfType<SceneManagement>().LoadSceneAsyncAfter(0, 1);
            });

            _btnClose.onClick.AddListener(() => {
                SetBehaviorActivation(false);
                Ui.DoHideWindow(_selfRectWindow, () => SetBehaviorActivation(true));
            });

            _btnShowAd.onClick.AddListener(OnBtnShowAd);
        }

        private void OnBtnShowAd() {
            SetBehaviorActivation(false); // remember to re-enable them again!

            if (_remainingAds > 0) {
                _ads.ShowRewardedVideo(
                    onAdsFinish: (result) => {
                        SetBehaviorActivation(true);
                        OnUnityAdsFinishCallback(result);
                    },
                    onAdsError: (error) => SetBehaviorActivation(true)
                );
            } else {
                _remainingAds = 0;
                _popup.ShowPopup(() => SetBehaviorActivation(true));
            }
        }

        private void OnCompleteRestartRequest() {
            ActiveSettingsWindow(false);
            Ui.DoShowWindow(_selfRectWindow, () => SetBehaviorActivation(true));
        }

        private void OnLevelChanged(int newLevel) {
            if (newLevel > 20)
                _remainingAds = 5;
            else if (newLevel > 16)
                _remainingAds = 4;
            else if (newLevel > 10)
                _remainingAds = 3;
            else if (newLevel > 2)
                _remainingAds = 2;
        }

        private void OnUnityAdsFinishCallback(UnityEngine.Advertisements.ShowResult result) {
            switch (result) {
                case UnityEngine.Advertisements.ShowResult.Failed:
                case UnityEngine.Advertisements.ShowResult.Skipped:
                    break;
                case UnityEngine.Advertisements.ShowResult.Finished:
                    --_remainingAds;
                    GameManager.instance.RequestToRestart(RestartMode.Reallocate, GenerateLife());
                    _selfRectWindow.gameObject.SetActive(false);
                    break;
            }
        }

        private byte GenerateLife() {
            return Convert.ToByte(Mathf.Clamp(GameManager.instance.currentLevel / 4, 2, 5));
        }

        private void ActiveSettingsWindow(bool active) {
            _settingsItem.SetActive(active);
            _closeItem.SetActive(active);
            _btnClose.enabled = active;
            _watchAdItem.SetActive(!active);
        }
    }
}