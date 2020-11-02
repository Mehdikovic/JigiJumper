using System.Linq;
using UnityEngine;


namespace JigiJumper.Data
{
    [CreateAssetMenu(fileName = "Probability Data", menuName = "Data/SpawnProbability")]
    public class SpawnProbabilities : ScriptableObject
    {
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

        public PlanetType GetPlanetType()
        {
            if (_types == null || _types.Length == 0) { return _defaultType; }

            int random = Random.Range(1, 101);
            int aggregation = 0;

            for (int i = 0; i < _types.Length; ++i)
            {
                var typeProbability = _types[i];
                aggregation += typeProbability.probability;
                if (random <= aggregation)
                {
                    return typeProbability.type;
                }
            }

            return _defaultType;
        }
        
        public float GetOscillationSpeed()
        {
            if (_oscillationSpeed == null || _oscillationSpeed.Length == 0) { return _defaultOscillaionSpeed; }

            int random = Random.Range(1, 101);
            int aggregation = 0;

            for (int i = 0; i < _oscillationSpeed.Length; ++i)
            {
                var speedProbability = _oscillationSpeed[i];
                aggregation += speedProbability.probability;
                if (random <= aggregation)
                {
                    return speedProbability.value;
                }
            }

            return _defaultOscillaionSpeed;
        }

        public float GetSelfDestructionTimer()
        {
            if (_selfDestructionTimer == null || _selfDestructionTimer.Length == 0) { return _defaultSelfDestructionTimer; }

            int random = Random.Range(1, 101);
            int aggregation = 0;

            for (int i = 0; i < _selfDestructionTimer.Length; ++i)
            {
                var selfDestructor = _selfDestructionTimer[i];
                aggregation += selfDestructor.probability;
                if (random <= aggregation)
                {
                    return selfDestructor.value;
                }
            }

            return _defaultSelfDestructionTimer;
        }

        public float GetRotationSpeed()
        {
            if (_rotationSpeed == null || _rotationSpeed.Length == 0) { return _defaultRotationSpeed; }

            int random = Random.Range(1, 101);
            int aggregation = 0;

            for (int i = 0; i < _rotationSpeed.Length; ++i)
            {
                var rotationSpeedProbability = _rotationSpeed[i];
                aggregation += rotationSpeedProbability.probability;
                if (random <= aggregation)
                {
                    return rotationSpeedProbability.value;
                }
            }

            return _defaultRotationSpeed;
        }

        public DestructionState GetState()
        {
            if (_state == null || _state.Length == 0) { return _defaultState; }

            int random = Random.Range(1, 101);
            int aggregation = 0;

            for (int i = 0; i < _state.Length; ++i)
            {
                var stateProbability = _state[i];
                aggregation += stateProbability.probability;
                if (random <= aggregation)
                {
                    return stateProbability.state;
                }
            }

            return _defaultState;
        }


        [System.Serializable]
        protected struct PlanetTypeProbability
        {
            public PlanetType type;
            public int probability;
        }

        [System.Serializable]
        protected struct ValueProbability
        {
            public float value;
            public int probability;
        }

        [System.Serializable]
        protected struct StateProbability
        {
            public DestructionState state;
            public int probability;
        }
    }
}