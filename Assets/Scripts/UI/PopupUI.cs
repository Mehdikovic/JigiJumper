using System;
using UnityEngine;
using UnityEngine.UI;

namespace JigiJumper.Ui {
    public class PopupUi : MonoBehaviour {
        [SerializeField] private RectTransform _container = null;
        [SerializeField] private Button _btnBack = null;

        Action _onPopupClosedCallback;

        private void Awake() {
            _btnBack.onClick.AddListener(() => {
                _btnBack.enabled = false;
                Ui.DoHideWindow(
                    _container,
                    () => {
                        _btnBack.enabled = true;
                        _onPopupClosedCallback?.Invoke();
                        _container.gameObject.SetActive(false);
                    }
                );
            });

            _container.gameObject.SetActive(false);
        }

        public void ShowPopup(Action onPopupClosed) {
            _onPopupClosedCallback = onPopupClosed;
            Ui.DoShowWindow(_container);
        }
    }
}
