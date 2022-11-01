using JigiJumper.Data;
using JigiJumper.Utils;
using TMPro;
using UnityEngine;

namespace JigiJumper.Ui {
    public class RecordItemUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _id = null;
        [SerializeField] private TextMeshProUGUI _total = null;
        [SerializeField] private TextMeshProUGUI _difficulty = null;
        [SerializeField] private TextMeshProUGUI _level = null;

        public void SetData(int index, RecordData data) {
            _id.text = string.Format("#{0}", index);
            _total.text = data.allJumpsCount.ToString();
            _difficulty.text = data.levelType.ToString();
            _level.text = data.level.ToString();
        }
    }
}