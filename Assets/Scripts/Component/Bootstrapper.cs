using JigiJumper.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

namespace JigiJumper.Component
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] SceneManagement _sceneManagement = null;
        [SerializeField] string gameId = "3872205"; // todo read from so
        [SerializeField] bool testMode = true;
        
        IEnumerator Start()
        {
            _sceneManagement.LoadSceneAsyncAfter(2.5f, 1);
            
            int counter = 0;
            var wait = new WaitForSeconds(.2f);
            
            Advertisement.Initialize(gameId, testMode);
            
            while (!Advertisement.isInitialized && counter < 10)
            {
                ++counter;
                yield return null;
            }
        }
    }
}