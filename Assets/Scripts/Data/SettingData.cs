using System.Collections;
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
        [Header("Level Type")]
        public LevelType levelType = LevelType.Easy;

        [Header("Configuration")]
        public bool showBanner = true;
        public int music = 100;
        public int gameSound = 100;
        
        [Header("Records")]
        public List<RecordData> records = new List<RecordData>();
    }
}