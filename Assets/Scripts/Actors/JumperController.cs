using System;
using Cinemachine;
using JigiJumper.Managers;
using UnityEngine;
using UnityEngine.EventSystems;


namespace JigiJumper.Actors
{
    public class JumperController : MonoBehaviour
    {
        public bool isInputSuspend = false;
        [SerializeField] private CinemachineVirtualCamera _cinemachine = null;

        private float _holdingPassedTime = 0f;
        private GameManager _gameManager;
        private PlanetController _currentPlanet;
        private PlanetController _previousPlanet;
        private int _remainingLife = 2;
        

        public event Action<PlanetController, PlanetController> OnPlanetReached;
        public event Action OnJump;
        public event Action<int> OnRestart;

        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            _gameManager = GameManager.instance;
        }

        private void Update()
        {
            HandleInput();

            if (_currentPlanet != null)
            {
                _transform.position = _currentPlanet.GetPivotCircuit().position;
                _transform.rotation = _currentPlanet.GetPivotCircuit().rotation;
            }
            else
            {
                float speed = _gameManager.GetSpawnProbabilities().GetJumperSpeed();
                _transform.Translate(Vector2.up * (Time.deltaTime * speed));
            }
        }

        public GameObject currentPlanetGameObject => (_currentPlanet != null) ? _currentPlanet.gameObject : null;
        public int remainingLife => _remainingLife;

        private void HandleInput()
        {
            if (isInputSuspend || EventSystem.current.IsPointerOverGameObject()) { return; }

            if (_currentPlanet == null) { return; }

            if (Input.GetMouseButton(0))
            {
                _holdingPassedTime += Time.deltaTime;
                if (_holdingPassedTime >= 1f)
                {
                    _currentPlanet.InvokeOnHoldingForJump();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _holdingPassedTime = 0f;
                _currentPlanet.InvokeOnJumperExit();
                _previousPlanet = _currentPlanet;
                _currentPlanet = null;
                OnJump?.Invoke();
            }
        }

        public void ReallocateYourself(byte addedLife)
        {
            _remainingLife = Mathf.Clamp(_remainingLife + addedLife, 0, 10);

            --_remainingLife;

            if (_remainingLife <= 0)
            {
                _remainingLife = 1;
                _gameManager.RequestToRestart(RestartMode.Destruction);
            }

            Restart();
        }

        private void Restart()
        {
            OnRestart?.Invoke(_remainingLife);
            if (_currentPlanet != null)
            {
                _currentPlanet.InvokeOnJumperPersistOnCurrentPlanetAfterRestart();
                return;
            }

            _previousPlanet.isVisited = false;
            PutJumperOnCircuit(_previousPlanet);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlanetController planetController = collision.GetComponent<PlanetController>();
            PutJumperOnCircuit(planetController);
        }

        private void PutJumperOnCircuit(PlanetController planetController)
        {
            if (planetController == null) { return; }
            if (planetController.isVisited) { return; }

            planetController.isVisited = true;
            _currentPlanet = planetController;

            Transform pivot = planetController.GetPivot();
            float angle = Vector2.SignedAngle(pivot.transform.up, _transform.position - pivot.position);
            pivot.localRotation *= Quaternion.Euler(Vector3.forward * angle);

            _cinemachine.Follow = planetController.transform;

            if (_previousPlanet != planetController)
            {
                OnPlanetReached?.Invoke(_previousPlanet, planetController);
            }
        }
    }
}