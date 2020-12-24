using UnityEngine;

using JigiJumper.Actors;
using JigiJumper.Utils;
using JigiJumper.Data;
using System;
using JigiJumper.Component;


namespace JigiJumper.Managers {
    public enum RestartMode { Reallocate, Destruction }
    public class GameManager : SingletonBehavior<GameManager> {
        [Header("Settings")]
        [SerializeField] private SettingData _setting = null;

        [Header("SpawnData")]
        [SerializeField] private SpawnProbabilities[] _spawnProbabilities = null;

        [Header("SavingController")]
        [SerializeField] private SavingController _saving = null;

        LazyValue<ManagePoints> _managePoints = new LazyValue<ManagePoints>(
                                                () => new ManagePoints());
        LazyValue<JumperController> _lazyJumper = new LazyValue<JumperController>(
                                                  () => FindObjectOfType<JumperController>());

        JumperController _jumper;


        public event Action<int> OnLevelChanged;
        public event Action OnCompleteRestartRequest;

        protected override void OnAwake() {
            _jumper = _lazyJumper.value;
            _jumper.OnPlanetReached += OnPlanetReached;
        }

        public LevelType levelType => _setting.levelType;

        public void RequestToRestart(RestartMode mode) {
            RequestToRestart(mode, 0);
        }

        public void RequestToRestart(RestartMode mode, byte addedLife) {
            switch (mode) {
                case RestartMode.Reallocate:
                    _jumper.ReallocateYourself(addedLife);
                    break;
                case RestartMode.Destruction:
                    OnCompleteRestartRequest?.Invoke();
                    break;
            }
        }

        public JumperController jumper => _lazyJumper.value;
        public int currentLevel => _managePoints.value.GetLevel();

        public SpawnProbabilities GetSpawnProbabilities() {
            int calculatedLevel = (_managePoints.value.GetLevel() - 1) + (int)_setting.levelType;
            int validIndex = Mathf.Clamp(calculatedLevel, 0, _spawnProbabilities.Length - 1);
            return _spawnProbabilities[validIndex];
        }

        public void RequestSelfDestructionPlanet(GameObject selfDestructorGameObject) {
            if (_jumper.currentPlanetGameObject != selfDestructorGameObject) { return; }

            // todo -> make animations
            RequestToRestart(RestartMode.Destruction);
        }

        private void OnPlanetReached(PlanetController _, PlanetController __) {
            if (_managePoints.value.AddPointToReachNextLevel()) {
                OnLevelChanged?.Invoke(_managePoints.value.GetLevel());
            }
        }

        private void OnDestroy() {
            if (_saving != null)
                _saving.Save(_managePoints.value.GetSavingRecordSession(_setting.levelType));
        }
    }
}