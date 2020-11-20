using System;
using UnityEngine;
using UnityEngine.Advertisements;


namespace JigiJumper.Ads
{
    public class RewardedAd : MonoBehaviour, IUnityAdsListener
    {
        [SerializeField] string gameId = "3872205";
        [SerializeField] string myPlacementId = "rewardedVideo";
        [SerializeField] bool testMode = true;

        private Action<ShowResult> _onAdsFinishCallback;
        private Action _onAdsStart;
        private Action _onAdsReady;
        private Action<string> _onAdsError;

        void Awake()
        {
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, testMode);
        }

        public void ShowRewardedVideo(
            Action<ShowResult> onAdsFinish,
            Action onAdsStart = null,
            Action onAdsReady = null,
            Action<string> onAdsError = null
        )
        {
            _onAdsFinishCallback = onAdsFinish;
            _onAdsStart = onAdsStart;
            _onAdsReady = onAdsReady;
            _onAdsError = onAdsError;

            if (!Advertisement.isInitialized || !Advertisement.IsReady())
            {
                _onAdsError?.Invoke("Ads didn't initialized");
                return;
            }

            Advertisement.Show(myPlacementId);
        }

        // Implement IUnityAdsListener interface methods:
        void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            _onAdsFinishCallback?.Invoke(showResult);
        }

        void IUnityAdsListener.OnUnityAdsReady(string placementId)
        {
            if (placementId == myPlacementId)
            {
                _onAdsReady?.Invoke();
            }
        }

        void IUnityAdsListener.OnUnityAdsDidError(string message)
        {
            _onAdsError?.Invoke(message);
        }

        void IUnityAdsListener.OnUnityAdsDidStart(string placementId)
        {
            _onAdsStart?.Invoke();
        }

        public void OnDestroy()
        {
            Advertisement.RemoveListener(this);
        }
    }
}