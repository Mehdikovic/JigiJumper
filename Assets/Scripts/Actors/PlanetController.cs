using JigiJumper.Component;
using JigiJumper.Data;
using System.Collections.Generic;
using UnityEngine;


namespace JigiJumper.Actors
{
    public class PlanetController : MonoBehaviour
    {
        //todo delete this line
        [SerializeField] private PlanetType planetType = PlanetType.Medium;
        [SerializeField] PlanetData _planetData = null;
        [SerializeField] private float _rotationSpeed = 50f;

        [SerializeField] private Transform _pivot = null;

        Transform _circuit;
        bool _isVisited = false;
        IEnumerable<IPlanetEventHandler> _receivers;

        private void Awake()
        {
            _circuit = _pivot.GetChild(0);
            _receivers = GetComponents<IPlanetEventHandler>();
        }

        private void LateUpdate()
        {
            HandleRotation();
        }

        // NOT Callbacks
        public bool isVisited { get => _isVisited; set => _isVisited = value; }
        
        public Transform GetPivot() => _pivot;

        public Transform GetPivotCircuit() => _circuit;

        public void InitialComponents(JumperController jumper)
        {
            //todo get info from probablility
            PlanetDataStructure data = _planetData.GetPlanetData(planetType);
            
            SetCircuitRadius(data.curcuitPosY);

            foreach (var receiver in _receivers)
            {
                receiver.OnInitialDataReceived(jumper, data);
            }
        }

        public void OnJumperEnter()
        {
            foreach (var receiver in _receivers)
            {
                receiver.OnJumperEnter();
            }
        }

        public void OnJumperExit()
        {
            _isVisited = false;

            foreach (var receiver in _receivers)
            {
                receiver.OnJumperExit();
            }
        }

        private void SetCircuitRadius(float curcuitPosY)
        {
            _circuit.localPosition = new Vector3(0, curcuitPosY, 0);
        }

        private void HandleRotation()
        {
            _pivot.Rotate(Vector3.forward * (Time.deltaTime * _rotationSpeed));
        }
    }
}