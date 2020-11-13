using GoogleMobileAds.Api;
using System;
using UnityEngine;


namespace JigiJumper.Managers
{
    public class GoogleAdManager : MonoBehaviour
    {
        private const string _unitID = "ca-app-pub-3940256099942544/5224354917";
        private RewardedAd _rewardedAd;

        public event Action UserWatchedTheAd;

        private void Awake()
        {
            MobileAds.Initialize(initStatus => { });

            _rewardedAd = new RewardedAd(_unitID);

            ConfigNewRewardAd();

            _rewardedAd.OnUserEarnedReward += (s, e) =>
            {
                string type = e.Type;
                double amount = e.Amount;
                print(
                     "HandleRewardedAdRewarded event received for "
                         + amount.ToString() + " " + type);
                
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
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded ad with the request.
            _rewardedAd.LoadAd(request);

        }
    }
}