using System;
using Cinemachine;
using UnityEngine;


namespace JigiJumper.Actors
{
    public class JumperController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _cinemachine = null;
        [SerializeField] private float _speed = 10f;

        PlanetController _currentPlanet;
        PlanetController _previousPlanet;

        public event Action<PlanetController, PlanetController> OnPlanetReached;
        

        private float _holdingPassedTime = 0f;

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
                transform.Translate(Vector2.up * (Time.deltaTime * _speed));
            }
        }

        public GameObject currentPlanetGameObject => (_currentPlanet != null) ? _currentPlanet.gameObject : null;

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.R))
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
            }


        }

        private void Restart()
        {
            if (_currentPlanet != null || _previousPlanet == null) { return; }

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