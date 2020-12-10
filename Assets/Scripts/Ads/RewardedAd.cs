using JigiJumper.Data;
using System;
using UnityEngine;
using UnityEngine.Advertisements;


namespace JigiJumper.Ads {
    public class RewardedAd : MonoBehaviour, IUnityAdsListener {
        [SerializeField] private SettingData _settings = null;
        [SerializeField] string myPlacementId = "rewardedVideo";

        private Action<ShowResult> _onAdsFinishCallback;
        private Action _onAdsStart;
        private Action _onAdsReady;
        private Action<string> _onAdsError;

        void Awake() {
            Advertisement.AddListener(this);
            Advertisement.Initialize(_settings.gameId, _settings.testMode);
        }

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

            if (!Advertisement.isInitialized || !Advertisement.IsReady()) {
                _onAdsError?.Invoke("Ads didn't initialized");
                return;
            }

            Advertisement.Show(myPlacementId);
        }

        // Implement IUnityAdsListener interface methods:
        void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
            _onAdsFinishCallback?.Invoke(showResult);
        }

        void IUnityAdsListener.OnUnityAdsReady(string placementId) {
            if (placementId == myPlacementId) {
                _onAdsReady?.Invoke();
            }
        }

        void IUnityAdsListener.OnUnityAdsDidError(string message) {
            _onAdsError?.Invoke(message);
        }

        void IUnityAdsListener.OnUnityAdsDidStart(string placementId) {
            _onAdsStart?.Invoke();
        }

        public void OnDestroy() {
            Advertisement.RemoveListener(this);
        }
    }
}