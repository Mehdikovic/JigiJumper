using System;
using Cinemachine;
using JigiJumper.Managers;
using UnityEngine;
using UnityEngine.EventSystems;


namespace JigiJumper.Actors
{
    public class JumperController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _cinemachine = null;

        private float _holdingPassedTime = 0f;
        private GameManager _gameManager;
        private PlanetController _currentPlanet;
        private PlanetController _previousPlanet;
        private int _remainingLife = 2;

        public event Action<PlanetController, PlanetController> OnPlanetReached;
        public event Action OnJump;
        public event Action OnRestart;

        private void Awake()
        {
            _gameManager = GameManager.instance;
        }

        private void Update()
        {
            HandleInput();

            if (_currentPlanet != null)
            {
                transform.position = _currentPlanet.GetPivotCircuit().position;
                transform.rotation = _currentPlanet.GetPivotCircuit().rotation;
            }
            else
            {
                float speed = _gameManager.GetSpawnProbabilities().GetJumperSpeed();
                transform.Translate(Vector2.up * (Time.deltaTime * speed));
            }
        }

        public GameObject currentPlanetGameObject => (_currentPlanet != null) ? _currentPlanet.gameObject : null;

        private void HandleInput()
        {
            if (EventSystem.current.IsPointerOverGameObject()) { return; }
            
            if (Input.GetKeyDown(KeyCode.R)) // todo debug purpose, delete it entirely
            {
                Restart();
            }

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

        public void ReallocateYourself()
        {
            if (_remainingLife <= 0)
            {
                _remainingLife = 0;
                _gameManager.RequestToRestart(RestartMode.Destruction);
            }

            --_remainingLife;
            Restart();
        }

        private void Restart()
        {
            if (_currentPlanet != null || _previousPlanet == null) { return; }
            OnRestart?.Invoke();
            
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
            float angle = Vector2.SignedAngle(pivot.transform.up, -transform.up);
            pivot.localRotation *= Quaternion.Euler(Vector3.forward * angle);

            _cinemachine.Follow = planetController.transform;

            if (_previousPlanet != planetController)
            {
                OnPlanetReached?.Invoke(_previousPlanet, planetController);
            }
        }
    }
}