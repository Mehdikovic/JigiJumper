using JigiJumper.Actors;
using JigiJumper.Data;
using JigiJumper.Managers;
using UnityEngine;


namespace JigiJumper.Component {
    public class Rotator : MonoBehaviour {
        [SerializeField] private Transform _pivot = null;

        private SpawnProbabilities _spawnProbabiliteis;
        private float _rotationSpeed;
        private float _storedRotationSpeed;

        private void Awake() {
            var planetController = GetComponent<PlanetController>();

            planetController.OnJumperEnter += () => SetupSpeed();

            planetController.OnJumperExit += () => {
                _rotationSpeed = _storedRotationSpeed;
            };

            planetController.OnJumperPersistOnPlanetAfterRestart += () => {
                _rotationSpeed = _storedRotationSpeed;
            };
        }

        void Update() {
            HandleRotation();
        }

        public void SetSpeedFactor(float factor) {
            _storedRotationSpeed = _rotationSpeed;
            _rotationSpeed *= factor;
        }

        private void SetupSpeed() {
            if (_spawnProbabiliteis == null) {
                _spawnProbabiliteis = GameManager.instance.GetSpawnProbabilities();
            }
            _rotationSpeed = _spawnProbabiliteis.GetRotationSpeed();
            _storedRotationSpeed = _rotationSpeed;
        }

        private void HandleRotation() {
            _pivot.Rotate(Vector3.forward * (Time.deltaTime * _rotationSpeed));
        }
    }
}