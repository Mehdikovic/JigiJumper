﻿using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace JigiJumper.Data
{
    [System.Serializable]
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

        public override string ToString()
        {
            return string.Format("Jumps: {0}, level: {1}, Difficulty: {2}",
                allJumpsCount,
                level,
                levelType.ToString()
            );
        }
    }

    [CreateAssetMenu(fileName = "Settings", menuName = "Data/Settings")]
    public class SettingData : ScriptableObject
    {
        [Header("Ads Conf")]
        [SerializeField] private string _gameId = "3872205";
        [SerializeField] private bool _testMode = true;

        [Header("Level Type")]
        public LevelType levelType = LevelType.Easy;

        //[Header("Records")]
        //public List<RecordData> records = new List<RecordData>();

        public string gameId => _gameId;
        public bool testMode => _testMode;

        public void SetMusicVol(float musicVol)
        {
            musicVol = Mathf.Clamp(musicVol, -80, 0);
            PlayerPrefs.SetFloat("musicVol", musicVol);
        }

        public float GetMusicVol()
        {
            if (!PlayerPrefs.HasKey("musicVol")) { return 0; }

            return PlayerPrefs.GetFloat("musicVol");
        }

        public void SetInGameSound(float inGameSoundVol)
        {
            inGameSoundVol = Mathf.Clamp(inGameSoundVol, -80, 0);
            PlayerPrefs.SetFloat("inGameSoundVol", inGameSoundVol);
        }

        public float GetInGameSound()
        {
            if (!PlayerPrefs.HasKey("inGameSoundVol")) { return 0; }

            return PlayerPrefs.GetFloat("inGameSoundVol");
        }

        public void SetShowBannerOption(bool value)
        {
            PlayerPrefs.SetInt("bannerShowOption", Convert.ToInt32(value));
        }

        public bool GetShowBannerOption()
        {
            if (!PlayerPrefs.HasKey("bannerShowOption")) { return true; }
            return Convert.ToBoolean(PlayerPrefs.GetInt("bannerShowOption"));
        }

        static public void SaveRecords(RecordData newRecord)
        {
            List<RecordData> oldRecords = LoadRecords();
            oldRecords.Add(newRecord);
            oldRecords = oldRecords
                .OrderByDescending(record => record.allJumpsCount)
                .Take(5)
                .ToList();

            using (var fs = System.IO.File.Open(GetRecordSavePath(), System.IO.FileMode.Create))
            {
                using (var writer = new BsonWriter(fs))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(writer, oldRecords);
                }
            }
        }


        static public List<RecordData> LoadRecords()
        {
            if (!System.IO.File.Exists(GetRecordSavePath())) { return new List<RecordData>(0); }
            List<RecordData> records;

            using (var fs = System.IO.File.OpenRead(GetRecordSavePath()))
            {
                using (var reader = new BsonReader(fs))
                {
                    reader.ReadRootValueAsArray = true;
                    var serializer = new JsonSerializer();
                    records = serializer.Deserialize<List<RecordData>>(reader);
                }
            }
            return records;
        }

        private static string GetRecordSavePath()
        {
            return Application.persistentDataPath + "/records.json";
        }
    }
}