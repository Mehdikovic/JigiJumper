using JigiJumper.Managers;
using UnityEngine;

namespace JigiJumper.Component
{
    public class Bootstrapper : MonoBehaviour
    {
        void Start()
        {
            FindObjectOfType<SceneManagement>().LoadSceneAsyncAfter(0, 1);
        }
    }
}