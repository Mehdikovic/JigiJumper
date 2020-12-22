using UnityEngine;

using JigiJumper.Data;
using JigiJumper.Managers;
using JigiJumper.Actors;

namespace JigiJumper.Component {
    public class SelfDestructor : MonoBehaviour {

        private GameManager _gameManager;
        private SpawnProbabilities _spawnProbabiliteis;
       
        private float _timer;
        private bool _isActivated;
        bool _isTimerStart = false;
        private DestructionState _internalState = DestructionState.None;
        private AudioSource _audio;
        private WaitForSeconds _wait;
        private Rotator _rotator;

        private void Awake() {
            _gameManager = GameManager.instance;
            _rotator = GetComponent<Rotator>();
            _audio = GetComponent<AudioSource>();
            _audio.loop = false;
            _audio.playOnAwake = false;
            _wait = new WaitForSeconds(1f);

            PlanetController planetController = GetComponent<PlanetController>();

            planetController.OnHoldingForJump += OnHoldForJumping;

            planetController.OnJumperEnter += OnJumperEnter;
            planetController.OnJumperExit += OnJumperExit;

            planetController.OnJumperPersistOnCurrentPlanetAfterRestart += () => Initialize();
        }

        private void OnHoldForJumping() {
            if (_internalState != DestructionState.OnHoldDestruction) { return; }
            if (_isActivated) { return; } // this had been activated before when the jumper enters. EXTRA CHECK, we don't realy need this because of internal state contoller
            _isActivated = true;
            _rotator.SetSpeedFactor(.4f);
        }

        private void Update() {
            if (_isActivated == false) { return; }

            _timer -= Time.deltaTime;

            TimeToStartTimer(_timer);

            if (_timer <= 0f) {
                _timer = 0f;
                _gameManager.RequestSelfDestructionPlanet(gameObject);
                _isActivated = false;
            }
        }

        public bool isActiveComponent => _isActivated;

        public float timer => _timer;

        public void OnJumperEnter() {
            Initialize();
        }

        private void Initialize() {
            _isActivated = false;
            SetupProbs();
            _internalState = _spawnProbabiliteis.GetState();
            _timer = _spawnProbabiliteis.GetSelfDestructionTimer();
            if (_internalState == DestructionState.OnEnterDestruction) {
                _isActivated = true;
            }
        }

        public void OnJumperExit() {
            _isActivated = false;
            _isTimerStart = false;
            _audio.Stop();
            StopAllCoroutines();
        }

        private void TimeToStartTimer(float timer) {
            if (_isTimerStart) { return; }
            if (Mathf.FloorToInt(timer) == 3) {
                _isTimerStart = true;
                StartCoroutine(PlayTimer(timer - 3f));
            }
        }

        System.Collections.IEnumerator PlayTimer(float wait) {
            yield return new WaitForSeconds(wait);
            _audio.Play();
            yield return _wait;
            _audio.Play();
            yield return _wait;
            _audio.Play();
        }

        private void SetupProbs() {
            if (_spawnProbabiliteis == null) {
                _spawnProbabiliteis = _gameManager.GetSpawnProbabilities();
            }
        }
    }
}