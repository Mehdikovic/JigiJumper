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
        private int _factorMultiplier = 1;
        private float _factor = 1.0f;

        private void Awake() {
            var planetController = GetComponent<PlanetController>();
            var lvlType = GameManager.instance.levelType;
            
            planetController.OnJumperEnter += () => SetupSpeed();

            planetController.OnJumperExit += () => {
                _rotationSpeed = _storedRotationSpeed;
            };

            planetController.OnJumperPersistOnCurrentPlanetAfterRestart += () => {
                _rotationSpeed = _storedRotationSpeed;
            };

            if (lvlType == LevelType.Easy) { return; }

            planetController.OnRestartAfterLastPlanet += () => {
                _factor += (_factorMultiplier++ * 0.1f);
                _rotationSpeed *= _factor;
                _rotationSpeed = Mathf.Clamp(_rotationSpeed, -220f, 220f);
                _storedRotationSpeed = _rotationSpeed;
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
            _factor = 1.0f;
            _factorMultiplier = 1;
        }

        private void HandleRotation() {
            _pivot.Rotate(Vector3.forward * (Time.deltaTime * _rotationSpeed));
        }
    }
}