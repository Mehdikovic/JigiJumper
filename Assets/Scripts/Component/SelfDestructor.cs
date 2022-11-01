using UnityEngine;

using JigiJumper.Data;
using JigiJumper.Managers;
using JigiJumper.Actors;
using System.Collections;
using System;

namespace JigiJumper.Component {
    public class SelfDestructor : MonoBehaviour {

        private GameManager _gm;
        private SpawnProbabilities _spawnProbabiliteis;
        private Rotator _rotator;

        private bool _isActivated;

        private DestructionState _internalState = DestructionState.None;

        private float _timer;
        private WaitForSeconds _wait;
        private Coroutine _timerUpdater;

        public event Action OnActivateTimer;
        public event Action OnDeactivateTimer;
        public event Action<int> OnTimerTick;

        private void Awake() {
            _wait = new WaitForSeconds(1f);
            _gm = GameManager.instance;
            _rotator = GetComponent<Rotator>();

            PlanetController planetController = GetComponent<PlanetController>();
            planetController.OnHoldingForJump += OnHoldForJumping;
            planetController.OnJumperEnter += OnJumperEnter;
            planetController.OnJumperExit += OnJumperExit;
            planetController.OnJumperPersistOnCurrentPlanetAfterRestart += () => Initialize();
            planetController.OnRestartAfterLastPlanet += () => Initialize();
        }

        private void OnHoldForJumping() {
            if (_internalState != DestructionState.OnHoldDestruction) { return; }
            if (_isActivated) { return; } // this had been activated before when the jumper enters. EXTRA CHECK, we don't realy need this because of internal state contoller
            ActivateTimer();
            _rotator.SetSpeedFactor(.4f);
        }

        public void OnJumperEnter() {
            Initialize();
        }

        private void Initialize() {
            DeactivateTimer();
            SetupProbs();
            _internalState = _spawnProbabiliteis.GetState();
            _timer = _spawnProbabiliteis.GetSelfDestructionTimer();

            if (_internalState == DestructionState.OnEnterDestruction) { ActivateTimer(); }
        }

        public void OnJumperExit() {
            DeactivateTimer();
        }

        private void SetupProbs() {
            if (_spawnProbabiliteis == null) {
                _spawnProbabiliteis = _gm.GetSpawnProbabilities();
            }
        }

        private void ActivateTimer() {
            _isActivated = true;
            if (_timerUpdater != null) {
                StopCoroutine(_timerUpdater);
            }
            OnActivateTimer?.Invoke();
            _timerUpdater = StartCoroutine(Updater());
        }

        private void DeactivateTimer() {
            OnDeactivateTimer?.Invoke();
            _isActivated = false;
            if (_timerUpdater != null) {
                StopCoroutine(_timerUpdater);
            }
            _timerUpdater = null;
        }

        IEnumerator Updater() {
            int remainingTime = Mathf.CeilToInt(_timer);
            do {
                OnTimerTick?.Invoke(remainingTime);
                yield return _wait;
                --remainingTime;
            } while (remainingTime >= 0);

            _gm.RequestSelfDestructionPlanet(gameObject);
            DeactivateTimer();
        }
    }
}