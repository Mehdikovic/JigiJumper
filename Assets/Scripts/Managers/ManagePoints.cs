using UnityEngine;
using JigiJumper.Utils;
using JigiJumper.Data;

namespace JigiJumper.Managers {
    public class ManagePoints {
        private const int LEVEL_DETR = 5;
        private int _point = 0;

        public int GetLevel() {
            return Mathf.Clamp((_point / LEVEL_DETR) + 1, 1, 999);
        }

        public bool AddPointToReachNextLevel() {
            ++_point;
            if (_point % LEVEL_DETR != 0) { return false; } // didn't reach next level
            return true; // reached next level
        }

        public RecordData GetSavingRecordSession(LevelType levelType) {
            return new RecordData {
                allJumpsCount = _point,
                level = GetLevel(),
                levelType = levelType,
            };
        }
    }
}
