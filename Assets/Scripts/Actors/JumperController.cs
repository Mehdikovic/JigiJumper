using Cinemachine;
using System;
using UnityEngine;

namespace JigiJumper.Actors
{
    public class JumperController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _cinemachine = null;
        [SerializeField] private float _speed = 10f;
        
        PlanetController _currentPlanet;
        PlanetController _oldPlanet;

        static public event Action<PlanetController, PlanetController> OnPlanetReached;

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

        private void HandleInput()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _oldPlanet = _currentPlanet;
                _currentPlanet = null;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlanetController planetController = collision.GetComponent<PlanetController>();
            
            if (planetController == null) { return; }

            if (planetController.isVisited) { return; }

            planetController.isVisited = true;

            _currentPlanet = planetController;

            Transform pivot = planetController.GetPivot();
            float angle = Vector2.SignedAngle(pivot.transform.up, -transform.up);
            pivot.localRotation *= Quaternion.Euler(Vector3.forward * angle);

            _cinemachine.Follow = planetController.transform;

            OnPlanetReached?.Invoke(_oldPlanet, planetController);
        }
    }
}