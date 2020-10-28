using UnityEngine;

using JigiJumper.Data;
using JigiJumper.Managers;
using JigiJumper.Actors;


namespace JigiJumper.Component
{
    public class SelfDestructor : MonoBehaviour
    {
        [Range(5f, 10f)]
        [SerializeField] private float _selfDestructionTimer = 5f;

        private PlanetController _planetController;
        private GameManager _gameManager;
        private JumperController _jumper;
        private float _timer;
        private bool _isActivated;
        private bool _isFirstPlanet = true;


        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _jumper = _gameManager.jumper;
            _planetController = GetComponent<PlanetController>();

            _planetController.OnSpawnedInitialization += OnSpawnedInitialization;
            _planetController.OnJumperEnter += OnJumperEnter;
            _planetController.OnJumperExit += OnJumperExit;
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
        
        public bool isActiveComponent => _isActivated;
        public float timer => _timer;

        // called before the JumperEnter and wouldn't be called for the first spawned object
        // ** when this function called, we're guaranteed that it's not the first planet who calls it **
        public void OnSpawnedInitialization(PlanetDataStructure data)
        {
            _isFirstPlanet = false;
            _timer = _selfDestructionTimer;
        }

        public void OnJumperEnter()
        {
            if (_isFirstPlanet) { return; }

            _isActivated = true; // todo read this from probability
        }

        public void OnJumperExit()
        {
            _isActivated = false;
        }
    }
}