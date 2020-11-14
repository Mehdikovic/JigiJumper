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

        public event Action<ShowResult> OnUnityAdsFinish;

        // Initialize the Ads listener and service:
        void Awake()
        {
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, testMode);
        }

        public void ShowRewardedVideo()
        {
            // Check if UnityAds ready before calling Show method:
            if (Advertisement.IsReady(myPlacementId))
            {
                Advertisement.Show(myPlacementId);
            }
            //else
            //{
                //Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
            //}
        }

        // Implement IUnityAdsListener interface methods:
        void IUnityAdsListener.OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            OnUnityAdsFinish?.Invoke(showResult);
        }

        void IUnityAdsListener.OnUnityAdsReady(string placementId)
        {
            // If the ready Placement is rewarded, show the ad:
            if (placementId == myPlacementId)
            {
                // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
            }
        }

        void IUnityAdsListener.OnUnityAdsDidError(string message)
        {
            // Log the error.
        }

        void IUnityAdsListener.OnUnityAdsDidStart(string placementId)
        {
            // Optional actions to take when the end-users triggers an ad.
        }

        // When the object that subscribes to ad events is destroyed, remove the listener:
        public void OnDestroy()
        {
            Advertisement.RemoveListener(this);
        }
    }
}