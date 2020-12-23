using JigiJumper.Data;
using JigiJumper.Managers;
using UnityEngine;

namespace JigiJumper.Component {
    public class SavingController : MonoBehaviour {
        void Awake() {
            GameManager.instance.OnTimeToSave += OnTimeToSave;
        }

        private void OnTimeToSave(RecordData records) {
            SettingData.SaveRecords(records);
        }
    }
}