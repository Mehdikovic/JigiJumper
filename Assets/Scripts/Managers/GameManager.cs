using UnityEngine;

using JigiJumper.Actors;
using JigiJumper.Utils;


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

        LazyValue<JumperController> _lazyJumper = new LazyValue<JumperController>( () => FindObjectOfType<JumperController>());
        JumperController _jumper;

        private void Awake()
        {
            _jumper = _lazyJumper.value;
        }

        public JumperController jumper => _jumper == null ? _lazyJumper.value: _jumper;

        public void RequestSelfDestructionPlanet(GameObject selfDestructorGameObject)
        {
            if (_jumper.currentPlanetGameObject != selfDestructorGameObject) { return; }

            print("destroyed");
        }
    }
}