using GoogleMobileAds.Api;
using System;
using UnityEngine;


namespace JigiJumper.Managers
{
    public class GoogleRewardAdManager : MonoBehaviour
    {
        private const string _unitID = "ca-app-pub-3940256099942544/5224354917";
        private RewardedAd _rewardedAd;

        public event Action UserWatchedTheAd;

        private void OnEnable()
        {
            _rewardedAd = new RewardedAd(_unitID);

            ConfigNewRewardAd();

            _rewardedAd.OnUserEarnedReward += (s, e) =>
            {
                UserWatchedTheAd?.Invoke();
            };

            _rewardedAd.OnAdClosed += (s, e) =>
            {
                ConfigNewRewardAd();
            };
        }

        public void UserChoseToWatchAd()
        {
            if (_rewardedAd.IsLoaded())
            {
                _rewardedAd.Show();
            }
        }

        private void ConfigNewRewardAd()
        {
            AdRequest request = new AdRequest.Builder().Build();
            _rewardedAd.LoadAd(request);

        }
    }
}