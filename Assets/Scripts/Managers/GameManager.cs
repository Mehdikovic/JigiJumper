using UnityEngine;

using JigiJumper.Actors;
using JigiJumper.Utils;
using JigiJumper.Data;
using System;
using JigiJumper.Component;
using System.Collections.Generic;

namespace JigiJumper.Managers {
    public enum RestartMode { Reallocate, Destruction, AfterAdWatched }
    public class GameManager : SingletonBehavior<GameManager> {
        [Header("Settings")]
        [SerializeField] private SettingData _setting = null;

        [Header("SpawnData")]
        [SerializeField] private SpawnProbabilities[] _spawnProbabilities = null;

        [Header("SavingController")]
        [SerializeField] private SavingController _saving = null;

        

        LazyValue<ManagePoints> _lazyManagePoints = new LazyValue<ManagePoints>(
                                                    () => new ManagePoints());

        LazyValue<JumperController> _lazyJumper = new LazyValue<JumperController>(
                                                  () => FindObjectOfType<JumperController>());

        JumperController _jumper;

        public event Action<int> OnLevelChanged;
        public event Action OnCompleteRestartRequest;

        private List<IInputHandler> _inputHandlers;

        protected override void OnAwake() {
            _inputHandlers = new List<IInputHandler>();
            _jumper = _lazyJumper.value;
            _jumper.OnPlanetReached += OnPlanetReached;
            _inputHandlers.Add(_jumper);
        }

        public LevelType levelType => _setting.levelType;

        public void RequestToRestart(RestartMode mode) {
            RequestToRestart(mode, 0);
        }

        public void RequestToRestart(RestartMode mode, byte addedLife) {
            switch (mode) {
                case RestartMode.Reallocate:
                    _jumper.ReallocateYourself(addedLife, mode);
                    break;
                case RestartMode.AfterAdWatched:
                    _jumper.ReallocateYourself(addedLife, mode);
                    break;
                case RestartMode.Destruction:
                    OnCompleteRestartRequest?.Invoke();
                    break;
            }
        }

        public JumperController jumper => _lazyJumper.value;
        public int currentLevel => _lazyManagePoints.value.GetLevel();

        public SpawnProbabilities GetSpawnProbabilities() {
            int calculatedLevel = (_lazyManagePoints.value.GetLevel() - 1) + (int)_setting.levelType;
            int validIndex = Mathf.Clamp(calculatedLevel, 0, _spawnProbabilities.Length - 1);
            return _spawnProbabilities[validIndex];
        }

        public void RequestSelfDestructionPlanet(GameObject selfDestructorGameObject) {
            if (_jumper.currentPlanetGameObject != selfDestructorGameObject) { return; }
            // todo -> make animations
            RequestToRestart(RestartMode.Destruction);
        }

        public void SuspendAllInputs() {
            foreach (var input in _inputHandlers) {
                input.SuspendInput();
            }
        }

        public void ReleaseAllInputs() {
            foreach (var input in _inputHandlers) {
                input.ReleaseInput();
            }
        }

        private void OnPlanetReached(PlanetController _, PlanetController __) {
            if (_lazyManagePoints.value.AddPointToReachNextLevel()) {
                OnLevelChanged?.Invoke(_lazyManagePoints.value.GetLevel());
            }
        }

        private void OnDestroy() {
            if (_saving != null)
                _saving.Save(_lazyManagePoints.value.GetSavingRecordSession(_setting.levelType));
        }
    }
}