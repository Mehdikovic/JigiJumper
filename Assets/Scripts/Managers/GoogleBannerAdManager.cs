using GoogleMobileAds.Api;
using UnityEngine;


namespace JigiJumper.Managers
{
    public class GoogleBannerAdManager : MonoBehaviour
    {
        private const string _unitID = "ca-app-pub-3940256099942544/6300978111";
        private BannerView _bannerViewAd;

        private void OnEnable()
        {
            _bannerViewAd = new BannerView(_unitID, AdSize.SmartBanner, AdPosition.Bottom);

            ConfigNewRewardAd();

            _bannerViewAd.OnAdClosed += (s, e) =>
            {
                ConfigNewRewardAd();
            };
        }

        private void ConfigNewRewardAd()
        {
            AdRequest request = new AdRequest.Builder().Build();
            _bannerViewAd.LoadAd(request);

        }

        private void OnDestroy()
        {
            _bannerViewAd.Destroy();
        }
    }
}