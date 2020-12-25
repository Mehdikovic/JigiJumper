using JigiJumper.Data;
using JigiJumper.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

namespace JigiJumper.Ads {
    public class BannerAd : PersistentBehavior<BannerAd> {

        [SerializeField] private SettingData _settings = null;
        [SerializeField] string placementId = "bannerPlacement";
#if UNITY_IOS || UNITY_ANDROID
        WaitForSeconds _wait;

        void Start() {
            if (!_settings.adEnable) { return; }

            Advertisement.Initialize(_settings.gameId, _settings.testMode);
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode _) {
            if (!Advertisement.isInitialized) { return; }
            if (scene.buildIndex == 1) {
                Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
                Advertisement.Banner.Show(placementId);
            }
        }
#endif
    }
}