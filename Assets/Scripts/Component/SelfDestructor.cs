using UnityEngine;

using JigiJumper.Data;
using JigiJumper.Actors;
using JigiJumper.Managers;


namespace JigiJumper.Component
{
    public class SelfDestructor : MonoBehaviour, IPlanetEventHandler
    {
        [Range(5f, 10f)]
        [SerializeField] private float _selfDestructionTimer = 5f;

        private float _timer;
        private JumperController _jumper;
        private bool _isActivated;
        private GameManager _gameManager;
        private bool _isFirstPlanet = true;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _jumper = _gameManager.jumper;
        }

        public bool isActiveComponent => _isActivated;
        public float timer => _timer;

        // called before the JumperEnter and wouldn't be called for the first spawned object
        // ** when this function called, we're guaranteed that it's not the first planet who calls it **
        public void OnNewSpawnedPlanetInitialization(PlanetDataStructure data)
        {
            _isFirstPlanet = false;
            _timer = _selfDestructionTimer;
        }

        private void Update()
        {
            if (_isActivated == false) { return; }

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = 0f;
                _gameManager.RequestSelfDestructionPlanet(gameObject);
                _isActivated = false;
            }
        }

        public void OnJumperEnter()
        {
            if (_isFirstPlanet) { return; }

            _isActivated = true; // todo read this from probability
        }

        public void OnDespawnedPreviousPlanet()
        {
            _isActivated = false;
        }

        public void OnJumperExit()
        {
            _isActivated = false;
            print("Stop selfDestruction planet " + name);
        }
    }
}