using JigiJumper.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

namespace JigiJumper.Component
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] string gameId = "3872205";
        [SerializeField] bool testMode = true;
        
        IEnumerator Start()
        {
            int counter = 0;
            var wait = new WaitForSeconds(.5f);
            
            Advertisement.Initialize(gameId, testMode);
            
            while (!Advertisement.isInitialized && counter < 4)
            {
                ++counter;
                yield return null;
                if (counter == 3)
                    print("isn't initailized");
            }

            FindObjectOfType<SceneManagement>().LoadSceneAsyncAfter(0, 1);
        }
    }
}