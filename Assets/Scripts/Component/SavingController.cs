using JigiJumper.Utils;
using UnityEngine;


namespace JigiJumper.Component {
    public class SavingController : MonoBehaviour {
        public void Save(RecordData records) {
            SavingUtility.SaveRecords(records);
        }
    }
}