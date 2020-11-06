using UnityEngine;

using JigiJumper.Actors;
using JigiJumper.Utils;
using JigiJumper.Data;
using System;

namespace JigiJumper.Managers
{
    public class GameManager : MonoBehaviour
    {
        private const int LEVEL_DETR = 5;
        #region Singleton
        static GameManager _instance;
        public static GameManager instance => GetInstance();
        static GameManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
        #endregion

        [SerializeField] private SpawnProbabilities[] _spawnProbabilities = null;

        LazyValue<JumperController> _lazyJumper = new LazyValue<JumperController>( () => FindObjectOfType<JumperController>());
        JumperController _jumper;

        private int _point = 0;
       
        public event Action<int> OnLevelChanged;
        
        private void Awake()
        {
            _jumper = _lazyJumper.value;
            _jumper.OnPlanetReached += OnPlanetReached;
        }

        public JumperController jumper => _lazyJumper.value;

        public SpawnProbabilities GetSpawnProbabilities()
        {
            int validIndex = Mathf.Clamp(GetLevel() - 1, 0, _spawnProbabilities.Length - 1);
            return _spawnProbabilities[validIndex];
        }

        public void RequestSelfDestructionPlanet(GameObject selfDestructorGameObject)
        {
            if (_jumper.currentPlanetGameObject != selfDestructorGameObject) { return; }

            print("destroyed");
        }

        private void OnPlanetReached(Actors.PlanetController arg1, Actors.PlanetController arg2)
        {
            ++_point;

            if (_point % LEVEL_DETR != 0) { return; }

            OnLevelChanged?.Invoke(GetLevel());
        }

        private int GetLevel()
        {
            return Mathf.Clamp(_point / LEVEL_DETR + 1, 1, 999);
        }
    }
}