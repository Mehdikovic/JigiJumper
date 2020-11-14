using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;


namespace JigiJumper.Ads
{
    public class BannerAd : MonoBehaviour
    {
        [SerializeField] string gameId = "3872205";
        [SerializeField] string placementId = "bannerPlacement";
        [SerializeField] bool testMode = true;

        WaitForSeconds _wait;

        void Awake()
        {
            _wait = new WaitForSeconds(0.5f);
            Advertisement.Initialize(gameId, testMode);
            StartCoroutine(ShowBannerWhenInitialized());
        }

        IEnumerator ShowBannerWhenInitialized()
        {
            while (!Advertisement.isInitialized)
            {
                yield return _wait;
            }
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show(placementId);
        }
    }
}