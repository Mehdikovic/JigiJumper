using JigiJumper.Data;
using JigiJumper.Managers;
using System;
using UnityEngine;


namespace JigiJumper.Actors
{
    public class PlanetController : MonoBehaviour
    {
        [SerializeField] PlanetData _planetData = null;
        [SerializeField] private Transform _pivot = null;

        private Transform _circuit;
        private bool _isVisited = false;
        private bool _isOnHoldForJumping = false;
        private SpawnProbabilities _spawnProbabilities;

        public event Action<PlanetDataStructure> OnSpawnedInitialization;
        public event Action OnJumperEnter;
        public event Action OnJumperExit;
        public event Action OnPlanetDespawned;
        public event Action OnHoldingForJump;


        private void Awake()
        {
            _circuit = _pivot.GetChild(0);
            _spawnProbabilities = GameManager.Instance.GetSpawnProbabilities();
        }

        public bool isVisited { get => _isVisited; set => _isVisited = value; }
        
        public Transform GetPivot() => _pivot;

        public Transform GetPivotCircuit() => _circuit;

        public void InvokeOnComponentInitialization()
        {
            //todo get info from probablility
            PlanetDataStructure data = _planetData.GetPlanetData(_spawnProbabilities.GetPlanetType());
            
            SetCircuitRadius(data.curcuitPosY);
            OnSpawnedInitialization?.Invoke(data);
        }

        public void InvokeOnJumperEnter()
        {
            _isOnHoldForJumping = false;
            OnJumperEnter?.Invoke();
        }

        public void InvokeOnJumperExit()
        {
            OnJumperExit?.Invoke();
        }

        public void InvokeOnPlanetDespawned()
        {
            _isVisited = false;
            OnPlanetDespawned?.Invoke();
        }

        public void InvokeOnHoldingForJump()
        {
            if (_isOnHoldForJumping) { return; }
            _isOnHoldForJumping = true;
            
            OnHoldingForJump?.Invoke();
        }

        private void SetCircuitRadius(float curcuitPosY)
        {
            _circuit.localPosition = new Vector3(0, curcuitPosY, 0);
        }
    }
}