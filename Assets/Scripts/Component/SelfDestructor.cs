using UnityEngine;

using JigiJumper.Data;
using JigiJumper.Managers;
using JigiJumper.Actors;
using System;

namespace JigiJumper.Component
{
    public class SelfDestructor : MonoBehaviour
    {
        [SerializeField] private Transform _pivot = null;

        private GameManager _gameManager;
        private PlanetController _planetController;
        private SpawnProbabilities _spawnProbabiliteis;
        
        private float _rotationSpeed;
        private float _storedRotationSpeed;
        private float _timer;
        private bool _isActivated;
        private bool _isFirstPlanet = true;
        private State _internalState = State.None;


        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _planetController = GetComponent<PlanetController>();
            _spawnProbabiliteis = _gameManager.GetSpawnProbabilities();

            _planetController.OnHoldingForJump += OnHoldForJumping;

            _planetController.OnSpawnedInitialization += OnSpawnedInitialization;
            _planetController.OnJumperEnter += OnJumperEnter;
            _planetController.OnJumperExit += OnJumperExit;
        }

        private void OnHoldForJumping()
        {
            if (_internalState != State.OnHoldDestruction) { return; }
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
            _timer = _spawnProbabiliteis.GetSelfDestructionTimer();
            _rotationSpeed = _spawnProbabiliteis.GetRotationSpeed();
        }

        public void OnJumperEnter()
        {
            if (_isFirstPlanet) {
                _rotationSpeed = _spawnProbabiliteis.GetRotationSpeed();
                _storedRotationSpeed = _rotationSpeed;
                return; 
            }

            _storedRotationSpeed = _rotationSpeed;

            _internalState = _spawnProbabiliteis.GetState();
            
            if (_internalState == State.OnEnterDestruction)
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