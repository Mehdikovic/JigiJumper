using UnityEngine;

using JigiJumper.Actors;
using JigiJumper.Utils;
using JigiJumper.Data;
using System;


namespace JigiJumper.Managers
{
    public enum RestartMode { Reallocate, Destruction }

    public class GameManager : SingletonBehavior<GameManager>
    {
        private const int LEVEL_DETR = 5;
        [Header("Settings")]
        [SerializeField] private SettingData _setting = null;

        [Header("SpawnData")]
        [SerializeField] private SpawnProbabilities[] _spawnProbabilities = null;

        LazyValue<JumperController> _lazyJumper = new LazyValue<JumperController>(() => FindObjectOfType<JumperController>());
        JumperController _jumper;

        private int _point = 0;

        public event Action<int> OnLevelChanged;
        public event Action OnCompleteRestartRequest;

        protected override void Awake()
        {
            base.Awake();

            _jumper = _lazyJumper.value;
            _jumper.OnPlanetReached += OnPlanetReached;
        }

        public LevelType levelType => _setting.levelType;

        public void RequestToRestart(RestartMode mode)
        {
            RequestToRestart(mode, 0);
        }

        public void RequestToRestart(RestartMode mode, byte addedLife)
        {
            switch (mode)
            {
                case RestartMode.Reallocate:
                    _jumper.ReallocateYourself(addedLife);
                    break;
                case RestartMode.Destruction:
                    OnCompleteRestartRequest?.Invoke();
                    break;
            }
        }

        public void SaveRecords()
        {
            SettingData.SaveRecords(
                new RecordData
                {
                    allJumpsCount = _point,
                    level = GetLevel(),
                    levelType = _setting.levelType,
                }
            );
        }

        public JumperController jumper => _lazyJumper.value;
        public int currentLevel => GetLevel();

        public SpawnProbabilities GetSpawnProbabilities()
        {
            int calculatedLevel = (GetLevel() - 1) + (int)_setting.levelType;
            int validIndex = Mathf.Clamp(calculatedLevel, 0, _spawnProbabilities.Length - 1);
            return _spawnProbabilities[validIndex];
        }

        public void RequestSelfDestructionPlanet(GameObject selfDestructorGameObject)
        {
            if (_jumper.currentPlanetGameObject != selfDestructorGameObject) { return; }

            // todo -> make animations
            RequestToRestart(RestartMode.Destruction);
        }

        private void OnPlanetReached(PlanetController arg1, PlanetController arg2)
        {
            ++_point;

            if (_point % LEVEL_DETR != 0) { return; }

            OnLevelChanged?.Invoke(GetLevel());
        }

        private int GetLevel()
        {
            return Mathf.Clamp((_point / LEVEL_DETR) + 1, 1, 999);
        }
    }
}