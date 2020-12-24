using JigiJumper.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JigiJumper.Utils {
    [System.Serializable]
    public struct RecordData {
        public LevelType levelType;
        public int level;
        public int allJumpsCount;

        public override string ToString() {
            return string.Format("Jumps: {0}, level: {1}, Difficulty: {2}",
                allJumpsCount,
                level,
                levelType.ToString()
            );
        }
    }

    public static class SavingUtility {
        static public void SaveRecords(RecordData newRecord) {
#if !UNITY_WEBGL
            List<RecordData> oldRecords = LoadRecords();
            oldRecords.Add(newRecord);
            oldRecords = oldRecords
                .OrderByDescending(record => record.levelType)
                .OrderByDescending(record => record.allJumpsCount)
                .Take(20)
                .ToList();

            using (var fs = System.IO.File.Open(GetRecordSavePath(), System.IO.FileMode.Create)) {
                using (var writer = new BsonWriter(fs)) {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(writer, oldRecords);
                }
            }
#endif
        }

        static public List<RecordData> LoadRecords() {
#if UNITY_WEBGL
            return new List<RecordData>(0);
#else
            if (!System.IO.File.Exists(GetRecordSavePath())) { return new List<RecordData>(0); }
            List<RecordData> records;

            using (var fs = System.IO.File.OpenRead(GetRecordSavePath())) {
                using (var reader = new BsonReader(fs)) {
                    reader.ReadRootValueAsArray = true;
                    var serializer = new JsonSerializer();
                    records = serializer.Deserialize<List<RecordData>>(reader);
                }
            }
            return records;
#endif
        }

        static string GetRecordSavePath() {
            return Application.persistentDataPath + "/records.json";
        }
    }
}