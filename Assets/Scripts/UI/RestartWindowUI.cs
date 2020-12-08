using JigiJumper.Ads;
using JigiJumper.Managers;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace JigiJumper.UI
{
    public class RestartWindowUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _container = null;
        [SerializeField] private PopupUI _popup = null;

        [Header("Reward-Ads")]
        [SerializeField] private RewardedAd _ads = null;

        [Header("Buttons")]
        [SerializeField] private Button _btnShowAd = null;
        [SerializeField] private Button _btnHome = null;
        [SerializeField] private Button _btnRestart = null;

        private int _remainingAds = 2;

        private void Awake()
        {
            InitialUIComponents();
            _container.gameObject.SetActive(false);
            GameManager.instance.OnCompleteRestartRequest += OnCompleteRestartRequest;
            GameManager.instance.OnLevelChanged += OnLevelChanged;
        }

        private void InitialUIComponents()
        {
            _btnRestart.onClick.AddListener(() =>
            {
                ActiveAllButtons(false);
                FindObjectOfType<SceneManagement>().LoadSceneAsyncAfter(0, SceneManager.GetActiveScene().buildIndex);
            });

            _btnHome.onClick.AddListener(() =>
            {
                ActiveAllButtons(false);
                FindObjectOfType<SceneManagement>().LoadSceneAsyncAfter(0, 1);
            });

            _btnShowAd.onClick.AddListener(OnBtnShowAd);
        }

        private void OnBtnShowAd()
        {
            ActiveAllButtons(false); // remember to re-enable them again!
            
            if (_remainingAds > 0)
            {
                _ads.ShowRewardedVideo(
                    onAdsFinish: (result) =>
                    {
                        ActiveAllButtons(true);
                        OnUnityAdsFinishCallback(result);
                    },
                    onAdsError: (error) => ActiveAllButtons(true)
                );
            }
            else
            {
                _remainingAds = 0;
                _popup.ShowPopup(() => ActiveAllButtons(true));
            }
        }

        private void OnCompleteRestartRequest()
        {
            Utils.DoTweenUtility.DoShowWindow(_container);
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
                    _container.gameObject.SetActive(false);
                    break;
            }
        }

        private byte GenerateLife()
        {
            return Convert.ToByte(Mathf.Clamp(GameManager.instance.currentLevel / 4, 2, 5));
        }

        private void ActiveAllButtons(bool activate)
        {
            _btnShowAd.enabled = activate;
            _btnHome.enabled = activate;
            _btnRestart.enabled = activate;
        }
    }
}