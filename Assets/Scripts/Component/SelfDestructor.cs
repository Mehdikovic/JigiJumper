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
        private float _timer;
        private bool _isActivated;
        private bool _isFirstPlanet = true;

        private float _storedRotationSpeed;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _planetController = GetComponent<PlanetController>();

            _planetController.OnHoldingForJump += OnHoldForJumping;

            _planetController.OnSpawnedInitialization += OnSpawnedInitialization;
            _planetController.OnJumperEnter += OnJumperEnter;
            _planetController.OnJumperExit += OnJumperExit;
        }

        private void OnHoldForJumping()
        {
            if (_isFirstPlanet) { return; } // we don't concer with another planet controller nor the first one
            if (_isActivated) { return; } // this had been activated before when the jumper enters.
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

            _isActivated = true; // todo read this from probability, activates as soon as jumper enters OR when the planet is holder jumper
        }

        public void OnJumperExit()
        {
            _isActivated = false;

            _rotationSpeed = _storedRotationSpeed;
        }
    }
}