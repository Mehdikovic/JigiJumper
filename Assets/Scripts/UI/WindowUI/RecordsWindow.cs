using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JigiJumper.UI
{
    public class RecordsWindow : WindowUI
    {
        [Header("Windows")]
        [SerializeField] private WindowUI _homeWindow = null;

        [Header("Ui Behavoirs")]
        [SerializeField] private Button _btnBack = null;
        [SerializeField] private Scrollbar _scrollbar = null;

        [Header("RecrodItemUi")]
        [SerializeField] private RecordItemUI[] _recordsUi = null;

        [Header("Viewport")]
        [SerializeField] private RectTransform _viewport = null;

        private void Awake()
        {
            InitialComponent();
            _selfRect.gameObject.SetActive(false);
        }

        private void InitialComponent()
        {
            _behaviorUIs = new Behaviour[]
            {
                _btnBack,
                _scrollbar,
            };

            DeactiveAllRecordUiItems();

            _btnBack.onClick.AddListener(() => TransitionToWindow(this, _homeWindow));
        }


        protected override void BeginToShow()
        {
            var records = Data.SettingData.LoadRecords();
            SetHeightOfRect(0);
            
            if (records.Count == 0) { return; }

            for (int i = 0; i < records.Count; i++)
            {
                _recordsUi[i].gameObject.SetActive(true);
                _recordsUi[i].SetData(i + 1, records[i]);
            }
            
            SetHeightOfRect(200 * records.Count);
        }

        private void SetHeightOfRect(float value)
        {
            var rect = _viewport.sizeDelta;
            rect.y = value;
            _viewport.sizeDelta = rect;
        }

        protected override void EndOfShow()
        {
            _scrollbar.value = 1;
        }

        protected override void EndOfHide()
        {
            DeactiveAllRecordUiItems();
        }

        private void DeactiveAllRecordUiItems()
        {
            foreach (var ui in _recordsUi)
            {
                ui.gameObject.SetActive(false);
            }

            StopAllCoroutines();
        }
    }
}