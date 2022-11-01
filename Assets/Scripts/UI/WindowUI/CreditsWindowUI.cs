using UnityEngine;
using UnityEngine.UI;

namespace JigiJumper.Ui {
    public class CreditsWindowUI : WindowUI {
        [Header("Windows")]
        [SerializeField] WindowUI _homeWindow = null;

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
            _btnBack.onClick.AddListener(() => UI.TransitionTo(this, _homeWindow));
        }
    }
}