using JigiJumper.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace JigiJumper.Ui {
    public class RecordsWindowUi : WindowUi {
        [Header("Windows")]
        [SerializeField] private WindowUi _homeWindow = null;

        [Header("Ui Behavoirs")]
        [SerializeField] private Button _btnBack = null;
        [SerializeField] private Scrollbar _scrollbar = null;

        [Header("RecrodItemUi")]
        [SerializeField] private RecordItemUi[] _recordsUi = null;

        [Header("Viewport")]
        [SerializeField] private RectTransform _viewport = null;

        protected override void OnAwake() {
            InitialComponent();
            
            SetBehaviorActivation(false);
            _selfRectWindow.gameObject.SetActive(false);
        }

        protected override Behaviour[] Behaviors() {
            return new Behaviour[] {
                _btnBack,
                _scrollbar,
            };
        }

        private void InitialComponent() {
            DeactiveAllRecordUiItems();

            _btnBack.onClick.AddListener(() => Ui.TransitionTo(this, _homeWindow));
        }

        public override void BeginToShow() {
            var records = SavingUtility.LoadRecords();
            SetHeightOfRect(0);

            if (records.Count == 0) { return; }

            for (int i = 0; i < records.Count; i++) {
                _recordsUi[i].gameObject.SetActive(true);
                _recordsUi[i].SetData(i + 1, records[i]);
            }

            SetHeightOfRect(150 * records.Count + 2);
        }

        private void SetHeightOfRect(float value) {
            var rect = _viewport.sizeDelta;
            rect.y = value;
            _viewport.sizeDelta = rect;
        }

        public override void EndOfShow() {
            _scrollbar.value = 1;
        }

        public override void EndOfHide() {
            DeactiveAllRecordUiItems();
        }

        private void DeactiveAllRecordUiItems() {
            foreach (var ui in _recordsUi) {
                ui.gameObject.SetActive(false);
            }

            StopAllCoroutines();
        }
    }
}