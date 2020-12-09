using JigiJumper.Data;
using TMPro;
using UnityEngine;

namespace JigiJumper.UI
{
    public class RecordItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _id = null;
        [SerializeField] private TextMeshProUGUI _total = null;
        [SerializeField] private TextMeshProUGUI _difficulty = null;
        [SerializeField] private TextMeshProUGUI _level = null;

        public void SetData(int index, RecordData data)
        {
            _id.text = string.Format("[{0}]", index);
            _total.text = data.allJumpsCount.ToString();
            _difficulty.text = data.levelType.ToString();
            _level.text = data.level.ToString();
        }
    }
}