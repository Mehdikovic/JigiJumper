using JigiJumper.Component;
using JigiJumper.Data;
using UnityEngine;


namespace JigiJumper.Actors
{
    public class PlanetController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 50f;

        [SerializeField] private Transform _pivot = null;

        Transform _circuit;
        bool _isVisited = false;
        Oscillator _oscillator;
        
        public bool isVisited { get => _isVisited; set => _isVisited = value; }

        private void Awake()
        {
            _circuit = _pivot.GetChild(0);
            _oscillator = GetComponent<Oscillator>();
            JumperController.OnPlanetReached += OnnewPlanetReached;
        }

        void LateUpdate()
        {
            HandleRotation();
        }

        void SetCircuitRadius(float curcuitPosY)
        {
            _circuit.localPosition = new Vector3(0, curcuitPosY, 0);
        }
        private void OnnewPlanetReached(PlanetController oldPlanet, PlanetController newRachedPlanet)
        {
            newRachedPlanet.GetComponent<Oscillator>().StopOscillattion();
        }

        private void HandleRotation()
        {
            _pivot.Rotate(Vector3.forward * (Time.deltaTime * _rotationSpeed));
        }

        public Transform GetPivot() => _pivot;

        public Transform GetPivotCircuit() => _circuit;

        public void ResetPlanet()
        {
            _isVisited = false;
            _oscillator.StopOscillattion();
        }

        public void Config(PlanetDataStructure data)
        {
            SetCircuitRadius(data.curcuitPosY);
            _oscillator.Init(data);
        }
    }
}