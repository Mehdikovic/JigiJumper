using UnityEngine;


namespace JigiJumper.Data {
    [CreateAssetMenu(fileName = "Probability Data", menuName = "Data/SpawnProbability")]
    public class SpawnProbabilities : ScriptableObject {
        [Header("Jumper Controller")]
        [SerializeField] private ValueProbability[] _jumperSpeed = null;
        [SerializeField] private float _defaultJumperSpeed = 10f;

        [Header("Planet Controller")]
        [SerializeField] private PlanetTypeProbability[] _types = null;
        [SerializeField] private PlanetType _defaultType = PlanetType.Large;

        [Header("Oscillator Controller")]
        [SerializeField] private ValueProbability[] _oscillationSpeed = null;
        [SerializeField] private float _defaultOscillaionSpeed = 0f;

        [Header("Destructor Controller")]
        [SerializeField] private ValueProbability[] _rotationSpeed = null;
        [SerializeField] private float _defaultRotationSpeed = 0f;
        [SerializeField] private StateProbability[] _state = null;
        [SerializeField] private DestructionState _defaultState = DestructionState.None;
        [SerializeField] private ValueProbability[] _selfDestructionTimer = null;
        [SerializeField] private float _defaultSelfDestructionTimer = 0f;

        public float GetJumperSpeed() {
            return GetValue(_jumperSpeed, _defaultJumperSpeed);
        }

        public float GetOscillationSpeed() {
            return GetValue(_oscillationSpeed, _defaultOscillaionSpeed);
        }

        public float GetSelfDestructionTimer() {
            return GetValue(_selfDestructionTimer, _defaultSelfDestructionTimer);
        }

        public float GetRotationSpeed() {
            int direction = Random.Range(0, 9) % 2 == 0 ? 1 : -1;
            return GetValue(_rotationSpeed, _defaultRotationSpeed * direction);
        }

        public PlanetType GetPlanetType() {
            if (_types == null || _types.Length == 0) { return _defaultType; }

            int random = Random.Range(1, 101);
            int aggregation = 0;

            for (int i = 0; i < _types.Length; ++i) {
                var typeProbability = _types[i];
                aggregation += typeProbability.probability;
                if (random <= aggregation) {
                    return typeProbability.type;
                }
            }

            return _defaultType;
        }

        public DestructionState GetState() {
            if (_state == null || _state.Length == 0) { return _defaultState; }

            int random = Random.Range(1, 101);
            int aggregation = 0;

            for (int i = 0; i < _state.Length; ++i) {
                var stateProbability = _state[i];
                aggregation += stateProbability.probability;
                if (random <= aggregation) {
                    return stateProbability.state;
                }
            }

            return _defaultState;
        }

        private float GetValue(ValueProbability[] valueList, float defalutValue) {
            if (valueList == null || valueList.Length == 0) { return defalutValue; }

            int random = Random.Range(1, 101);
            int aggregation = 0;

            for (int i = 0; i < valueList.Length; ++i) {
                var valueProbability = valueList[i];
                aggregation += valueProbability.probability;
                if (random <= aggregation) {
                    return valueProbability.value;
                }
            }

            return defalutValue;
        }

        [System.Serializable]
        public struct PlanetTypeProbability {
            public PlanetType type;
            public int probability;
        }

        [System.Serializable]
        public struct ValueProbability {
            public float value;
            public int probability;
        }

        [System.Serializable]
        public struct StateProbability {
            public DestructionState state;
            public int probability;
        }
    }
}