using JigiJumper.Data;
using System;
using UnityEngine;
using UnityEngine.Advertisements;


namespace JigiJumper.Ads {
    public class RewardedAd : MonoBehaviour, IUnityAdsListener {
        [SerializeField] private SettingData _settings = null;
        [SerializeField] string rewardedId_Android = "Rewarded_Android";
        [SerializeField] string rewardedId_iOS = "Rewarded_iOS";

        private Action<ShowResult> _onAdsFinishCallback;
        private Action _onAdsStart;
        private Action _onAdsReady;
        private Action<string> _onAdsError;

        private string _rewardedId;

#if UNITY_IOS || UNITY_ANDROID
        void Awake() {
            if (!_settings.adEnable) { return; }
#if UNITY_ANDROID
            _rewardedId = rewardedId_Android;
#elif UNITY_IOS
            _rewardedId = rewardedId_IOS;
#endif

            Advertisement.AddListener(this);
            Advertisement.Initialize(_settings.gameId, _settings.testMode);
        }
#endif

        public void ShowRewardedVideo(
            Action<ShowResult> onAdsFinish,
            Action onAdsStart = null,
            Action onAdsReady = null,
            Action<string> onAdsError = null
        ) {

            _onAdsFinishCallback = onAdsFinish;
            _onAdsStart = onAdsStart;
            _onAdsReady = onAdsReady;
            _onAdsError = onAdsError;
#if UNITY_IOS || UNITY_ANDROID
            if (!_settings.adEnable) {
                onAdsFinish?.Invoke(ShowResult.Finished);
                return;
            }

            if (!Advertisement.isInitialized || !Advertisement.IsReady()) {
                _onAdsError?.Invoke("Ads didn't initialized");
                return;
            }

            Advertisement.Show(_rewardedId);
#elif UNITY_WEBGL
            _onAdsError?.Invoke("Ads didn't initialized");
#endif
        }

        // Implement IUnityAdsListener interface methods:
        void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
            _onAdsFinishCallback?.Invoke(showResult);
        }

        void IUnityAdsListener.OnUnityAdsReady(string placementId) {
            if (placementId == _rewardedId) {
                _onAdsReady?.Invoke();
            }
        }

        void IUnityAdsListener.OnUnityAdsDidError(string message) {
            _onAdsError?.Invoke(message);
        }

        void IUnityAdsListener.OnUnityAdsDidStart(string placementId) {
            _onAdsStart?.Invoke();
        }
#if UNITY_IOS || UNITY_ANDROID
        public void OnDestroy() {
            Advertisement.RemoveListener(this);
        }
#endif
    }
}