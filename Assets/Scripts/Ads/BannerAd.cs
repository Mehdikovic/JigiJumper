using JigiJumper.Data;
using JigiJumper.Managers;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;


namespace JigiJumper.Ads {
    public class BannerAd : PersistentBehavior<BannerAd> {

        [SerializeField] private SettingData _settings = null;
        [SerializeField] string bannerId_Android = "Banner_Android";
        [SerializeField] string bannerId_iOS = "Banner_iOS";

#if UNITY_IOS || UNITY_ANDROID
        WaitForSeconds _wait;
        string _bannerId;

        void Start() {
            if (!_settings.adEnable) { return; }

#if UNITY_ANDROID
            _bannerId = bannerId_Android;
#elif UNITY_IOS
            bannerId = bannerId_IOS;
#endif
            Advertisement.Initialize(_settings.gameId, _settings.testMode);
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode _) {
            if (!Advertisement.isInitialized) { return; }
            if (scene.buildIndex == 1) {
                Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
                Advertisement.Banner.Show(_bannerId);
            }
        }
#endif
    }
}