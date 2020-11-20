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

        void Start()
        {
            _wait = new WaitForSeconds(.5f);
            StartCoroutine(ShowBannerWhenInitialized());
        }

        IEnumerator ShowBannerWhenInitialized()
        {
            //yield return _wait;
            
            Advertisement.Initialize(gameId, testMode);
            
            while (!Advertisement.isInitialized)
            {
                yield return _wait;
            }
            print("Called");
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show(placementId);
        }
    }
}