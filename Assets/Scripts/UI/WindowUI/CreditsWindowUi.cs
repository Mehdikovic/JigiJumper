using UnityEngine;
using UnityEngine.UI;

namespace JigiJumper.Ui {
    public class CreditsWindowUi : WindowUi {
        [Header("Windows")]
        [SerializeField] WindowUi _homeWindow = null;

        [Header("Behaviors")]
        [SerializeField] Button _btnBack = null;

        protected override void OnAwake() {
            InitialComponents();

            SetBehaviorActivation(false);
            _selfRectWindow.gameObject.SetActive(false);
        }

        protected override Behaviour[] Behaviors() {
            return new Behaviour[] {
                _btnBack,
            };
        }

        private void InitialComponents() {
            _btnBack.onClick.AddListener(() => Ui.TransitionTo(this, _homeWindow));
        }
    }
}