using JigiJumper.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;


namespace JigiJumper.Ads {
    public class BannerAd : MonoBehaviour {

        [SerializeField] private SettingData _settings = null;
        [SerializeField] string placementId = "bannerPlacement";

        WaitForSeconds _wait;

        IEnumerator Start() {
            Advertisement.Initialize(_settings.gameId, _settings.testMode);
            _wait = new WaitForSeconds(0.5f);
            yield return ShowBannerWhenInitialized();
        }

        IEnumerator ShowBannerWhenInitialized() {
            while (!Advertisement.isInitialized) {
                yield return _wait;
            }
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show(placementId);
        }
    }
}