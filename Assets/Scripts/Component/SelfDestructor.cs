using UnityEngine;

using JigiJumper.Data;
using JigiJumper.Managers;
using JigiJumper.Actors;


namespace JigiJumper.Component
{
    public class SelfDestructor : MonoBehaviour
    {
        [SerializeField] private Transform _pivot = null;

        private GameManager _gameManager;
        private SpawnProbabilities _spawnProbabiliteis;

        private float _rotationSpeed;
        private float _storedRotationSpeed;
        private float _timer;
        private bool _isActivated;
        private bool _isFirstPlanet = true;
        private DestructionState _internalState = DestructionState.None;


        private void Awake()
        {
            _gameManager = GameManager.instance;

            PlanetController planetController = GetComponent<PlanetController>();

            planetController.OnHoldingForJump += OnHoldForJumping;

            planetController.OnSpawnedInitialization += OnSpawnedInitialization;
            planetController.OnJumperEnter += OnJumperEnter;
            planetController.OnJumperExit += OnJumperExit;

            planetController.OnJumperPersistOnPlanetAfterRestart += () =>
            {
                _isActivated = false;
                _rotationSpeed = _storedRotationSpeed;
            };
        }

        private void OnHoldForJumping()
        {
            if (_internalState != DestructionState.OnHoldDestruction) { return; }
            if (_isFirstPlanet) { return; } // we don't concer with the first one
            if (_isActivated) { return; } // this had been activated before when the jumper enters. EXTRA CHECK, we don't realy need this because of internal state contoller
            _isActivated = true;
            _rotationSpeed *= .4f;
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

        private void LateUpdate()
        {
            HandleRotation();
        }

        private void HandleRotation()
        {
            _pivot.Rotate(Vector3.forward * (Time.deltaTime * _rotationSpeed));
        }

        public bool isActiveComponent => _isActivated;

        public float timer => _timer;

        // called before the JumperEnter and wouldn't be called for the first spawned object
        // ** when this function called, we're guaranteed that it's not the first planet who calls it **
        public void OnSpawnedInitialization(PlanetDataStructure data)
        {
            _isFirstPlanet = false;
            _spawnProbabiliteis = _gameManager.GetSpawnProbabilities();
            
            _timer = _spawnProbabiliteis.GetSelfDestructionTimer();
            _rotationSpeed = _spawnProbabiliteis.GetRotationSpeed();
        }

        public void OnJumperEnter()
        {
            if (_isFirstPlanet) {
                _spawnProbabiliteis = _gameManager.GetSpawnProbabilities();
                _rotationSpeed = _spawnProbabiliteis.GetRotationSpeed();
                _storedRotationSpeed = _rotationSpeed;
                return; 
            }

            _storedRotationSpeed = _rotationSpeed;

            _internalState = _spawnProbabiliteis.GetState();
            
            if (_internalState == DestructionState.OnEnterDestruction)
            {
                _isActivated = true;
            }
        }

        public void OnJumperExit()
        {
            _isActivated = false;
            _rotationSpeed = _storedRotationSpeed; // because of restart system we need to store previous value of rotation
        }
    }
}