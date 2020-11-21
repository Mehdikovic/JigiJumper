using System.Collections.Generic;
using UnityEngine;


namespace JigiJumper.Data
{
    public enum LevelType
    {
        Easy = 0,
        Normal = 10,
        Hard = 20,
    }

    [System.Serializable]
    public struct RecordData
    {
        public LevelType levelType;
        public int level;
        public int allJumpsCount;
    }

    [CreateAssetMenu(fileName = "Settings", menuName = "Data/Settings")]   
    public class SettingData : ScriptableObject
    {
        [Header("Ads Conf")]
        [SerializeField] private string _gameId = "3872205";
        [SerializeField] private bool _testMode = true;
        
        [Header("Level Type")]
        public LevelType levelType = LevelType.Easy;

        [Header("Configuration")]
        public bool showBanner = true;

        [Header("Mixer Sound")]
        [Range(-80f, 0f)] public float music = 0;
        [Range(-80f, 0f)] public float inGameSound = 0;

        [Header("Records")]
        public List<RecordData> records = new List<RecordData>();

        public string gameId => _gameId;
        public bool testMode => _testMode;
    }
}