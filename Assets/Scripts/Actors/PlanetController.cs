using JigiJumper.Data;
using JigiJumper.Managers;
using System;
using UnityEngine;


namespace JigiJumper.Actors {
    public class PlanetController : MonoBehaviour {
        [SerializeField] PlanetData _planetData = null;
        [SerializeField] private Transform _pivot = null;
        [SerializeField] private SpriteRenderer _circleSpriteRenderer = null;
        [SerializeField] private SpriteRenderer _selfSpriteRenderer = null;

        private Transform _circuit;
        private bool _isVisited = false;
        private bool _isOnHoldForJumping = false;
        private PlanetType _planetType = PlanetType.Large;

        public event Action<PlanetDataStructure> OnSpawnedInitialization;
        public event Action OnJumperEnter;
        public event Action OnJumperExit;
        public event Action OnPlanetDespawned;
        public event Action OnHoldingForJump;
        public event Action OnJumperPersistOnCurrentPlanetAfterRestart;


        private void Awake() {
            _circuit = _pivot.GetChild(0);
        }

        public bool isVisited { get => _isVisited; set => _isVisited = value; }

        public Color GetSpriteTint() => _circleSpriteRenderer.color;

        public PlanetType PlanetType => _planetType;

        public void SetSpriteColor(Color color) {
            _circleSpriteRenderer.color = color;
            _selfSpriteRenderer.color = color;
        }

        public Transform GetPivot() => _pivot;

        public Transform GetPivotCircuit() => _circuit;

        // called before the JumperEnter and wouldn't be called for the first spawned object
        // ** when this function called, we're guaranteed that it's not the first planet who calls it **
        public void InvokeOnComponentInitialization() {
            var spawnProbabilities = GameManager.instance.GetSpawnProbabilities();
            _planetType = spawnProbabilities.GetPlanetType();
            PlanetDataStructure data = _planetData.GetPlanetData(_planetType);

            SetCircuitRadius(data.curcuitPosY);
            OnSpawnedInitialization?.Invoke(data);
        }

        public void InvokeOnJumperEnter() {
            _isOnHoldForJumping = false;
            OnJumperEnter?.Invoke();
        }

        public void InvokeOnJumperExit() {
            OnJumperExit?.Invoke();
        }

        public void InvokeOnPlanetDespawned() {
            _isVisited = false;
            OnPlanetDespawned?.Invoke();
        }

        public void InvokeOnHoldingForJump() {
            if (_isOnHoldForJumping) { return; }
            _isOnHoldForJumping = true;

            OnHoldingForJump?.Invoke();
        }

        private void SetCircuitRadius(float curcuitPosY) {
            _circuit.localPosition = new Vector3(0, curcuitPosY, 0);
        }

        public void InvokeOnJumperPersistOnCurrentPlanetAfterRestart() {
            OnJumperPersistOnCurrentPlanetAfterRestart?.Invoke();
        }
    }
}