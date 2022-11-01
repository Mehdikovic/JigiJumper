using System.Collections.Generic;
using UnityEngine;


namespace JigiJumper.Data {
    public enum PlanetType { Large, Medium, Small, Tiny }
    public enum DestructionState { None, OnEnterDestruction, OnHoldDestruction }

    [System.Serializable]
    public class PlanetDataStructure {
        public PlanetType type;
        public float radius;
        public float curcuitPosY;
        public Vector2 xRange;
        public Vector2 yRange;
    }

    [CreateAssetMenu(fileName = "Planet Data", menuName = "Data/Planet")]
    public class PlanetData : ScriptableObject, ISerializationCallbackReceiver {
        [SerializeField] private PlanetDataStructure[] _planetsData = null;
        Dictionary<PlanetType, PlanetDataStructure> _dataMap;

        public PlanetDataStructure GetPlanetData(PlanetType type) {
            return _dataMap[type];
        }

        public void OnBeforeSerialize() {
        }

        public void OnAfterDeserialize() {
            if (_dataMap == null) {
                _dataMap = new Dictionary<PlanetType, PlanetDataStructure>();
            }

            _dataMap.Clear();

            for (int i = 0; i < _planetsData.Length; ++i) {
                _dataMap.Add(_planetsData[i].type, _planetsData[i]);
            }
        }
    }
}