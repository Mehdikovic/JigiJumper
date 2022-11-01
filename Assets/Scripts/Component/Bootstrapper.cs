using JigiJumper.Data;
using JigiJumper.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;


namespace JigiJumper.Component {
    public class Bootstrapper : MonoBehaviour {
        [SerializeField] SceneManagement _sceneManagement = null;
        [SerializeField] SettingData _settings = null;

        IEnumerator Start() {
            _sceneManagement.LoadSceneAsyncAfter(2.5f, 1);
#if UNITY_IOS || UNITY_ANDROID
            int counter = 0;
            var wait = new WaitForSeconds(.2f);

            Advertisement.Initialize(_settings.gameId, _settings.testMode);

            while (!Advertisement.isInitialized && counter < 10) {
                ++counter;
                yield return wait;
            }
#elif UNITY_WEBGL
            yield return null;
#else
            yield return null;
#endif
        }
    }
}
