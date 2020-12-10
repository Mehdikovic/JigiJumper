using JigiJumper.Ads;
using JigiJumper.Managers;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace JigiJumper.UI
{
    public class MainGameWindowUi : WindowUi
    {
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

        private void Awake()
        {
            _gameManager = GameManager.instance;
            InitialUIComponents();
            _selfRectWindow.gameObject.SetActive(false);
            _gameManager.OnCompleteRestartRequest += OnCompleteRestartRequest;
            _gameManager.OnLevelChanged += OnLevelChanged;
        }

        public void ActivateSettingMenuWindow()
        {
            if (_isActivatingSettingWindow) { return; }

            _isActivatingSettingWindow = true;
            ActiveSettingsWindow(true);
            Utils.DoTweenUtility.DoShowWindow(_selfRectWindow, () => _isActivatingSettingWindow = false);
        }

        private void InitialUIComponents()
        {
            _uiBehaviors = new Behaviour[]
            {
                _btnShowAd,
                _btnHome,
                _btnRestart,
                _btnShowSettings,
                _btnClose,
            };

            _btnShowSettings.onClick.AddListener(() =>
            {
                SetActivation(false, _uiBehaviors);
                Utils.DoTweenUtility.DoShowWindow(_settingsWindow.GetRectWindow(), () => SetActivation(true, _uiBehaviors));
            });

            _btnRestart.onClick.AddListener(() =>
            {
                SetActivation(false, _uiBehaviors);
                _gameManager.SaveRecords();
                FindObjectOfType<SceneManagement>().LoadSceneAsyncAfter(0, SceneManager.GetActiveScene().buildIndex);
            });

            _btnHome.onClick.AddListener(() =>
            {
                SetActivation(false, _uiBehaviors);
                _gameManager.SaveRecords();
                FindObjectOfType<SceneManagement>().LoadSceneAsyncAfter(0, 1);
            });

            _btnClose.onClick.AddListener(() =>
            {
                SetActivation(false, _uiBehaviors);
                Utils.DoTweenUtility.DoHideWindow(_selfRectWindow, () => SetActivation(true, _uiBehaviors));
            });

            _btnShowAd.onClick.AddListener(OnBtnShowAd);
        }

        private void OnBtnShowAd()
        {
            SetActivation(false, _uiBehaviors); // remember to re-enable them again!

            if (_remainingAds > 0)
            {
                _ads.ShowRewardedVideo(
                    onAdsFinish: (result) =>
                    {
                        SetActivation(true, _uiBehaviors);
                        OnUnityAdsFinishCallback(result);
                    },
                    onAdsError: (error) => SetActivation(true, _uiBehaviors)
                );
            }
            else
            {
                _remainingAds = 0;
                _popup.ShowPopup(() => SetActivation(true, _uiBehaviors));
            }
        }

        private void OnCompleteRestartRequest()
        {
            ActiveSettingsWindow(false);
            Utils.DoTweenUtility.DoShowWindow(_selfRectWindow);
        }

        private void OnLevelChanged(int newLevel)
        {
            if (newLevel > 20)
                _remainingAds = 5;
            else if (newLevel > 16)
                _remainingAds = 4;
            else if (newLevel > 10)
                _remainingAds = 3;
            else if (newLevel > 2)
                _remainingAds = 2;
        }

        private void OnUnityAdsFinishCallback(UnityEngine.Advertisements.ShowResult result)
        {
            switch (result)
            {
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

        private byte GenerateLife()
        {
            return Convert.ToByte(Mathf.Clamp(GameManager.instance.currentLevel / 4, 2, 5));
        }

        private void ActiveSettingsWindow(bool active)
        {
            _settingsItem.SetActive(active);
            _closeItem.SetActive(active);
            _btnClose.enabled = active;
            _watchAdItem.SetActive(!active);
        }
    }
}