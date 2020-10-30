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
        [SerializeField] private float _rotationSpeed = 50f;
        [SerializeField] private Transform _pivot = null;

        private PlanetController _planetController;
        private GameManager _gameManager;
        private JumperController _jumper;
        private float _timer;
        private bool _isActivated;
        private bool _isFirstPlanet = true;
        private bool _isOnHoldForJumping = false;

        private float _storedRotationSpeed;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _jumper = _gameManager.jumper;
            _planetController = GetComponent<PlanetController>();

            _jumper.OnHoldForJumping += OnHoldForJumping;

            _planetController.OnSpawnedInitialization += OnSpawnedInitialization;
            _planetController.OnJumperEnter += OnJumperEnter;
            _planetController.OnJumperExit += OnJumperExit;
        }

        private void OnHoldForJumping(PlanetController currentJumperPlanet)
        {
            // we don't concer with another planet controller nor the first one
            if (_isFirstPlanet || currentJumperPlanet != _planetController) { return; }
            
            if (_isActivated) { return; } // this had been activated before when the jumper enters.

            if (_isOnHoldForJumping) { return; } // a control flag to run this callback just once

            _isOnHoldForJumping = true;
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
            _timer = _selfDestructionTimer;
        }

        public void OnJumperEnter()
        {
            _storedRotationSpeed = _rotationSpeed;
            
            if (_isFirstPlanet) { return; }
            
            _isActivated = false; // todo read this from probability, activates as soon as jumper enters OR when the planet is holder jumper
            _isOnHoldForJumping = false;
        }

        public void OnJumperExit()
        {
            _isActivated = false;

            _rotationSpeed = _storedRotationSpeed;
        }
    }
}