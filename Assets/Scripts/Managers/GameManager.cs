using JigiJumper.Actors;
using UnityEngine;


namespace JigiJumper.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        static GameManager _instance;
        public static GameManager Instance => GetInstance();
        static GameManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
        #endregion

        JumperController _jumper;

        private void Awake()
        {
            _jumper = FindObjectOfType<JumperController>();
        }

        public void RequestSelfDestructionPlanet(GameObject selfDestructorGameObject)
        {
            if (_jumper.currentPlanetGameObject != selfDestructorGameObject) { return; }

            print("destroyed");
        }
    }
}