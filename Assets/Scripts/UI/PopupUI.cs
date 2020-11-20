using System;
using UnityEngine;
using UnityEngine.UI;

namespace JigiJumper.UI
{
    public class PopupUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _container = null;
        [SerializeField] private Button _btnBack = null;

        Action _onPopupClosedCallback;

        private void Awake()
        {
            _btnBack.onClick.AddListener(() =>
            {
                _btnBack.enabled = false;
                Utils.DoTweenUtility.DoHideWindow(
                    _container,
                    () =>
                    {
                        _btnBack.enabled = true;
                        _onPopupClosedCallback?.Invoke();
                        _container.gameObject.SetActive(false);
                    }
                );
            });

            _container.gameObject.SetActive(false);
        }

        public void ShowPopup(Action onPopupClosed)
        {
            _onPopupClosedCallback = onPopupClosed;
            Utils.DoTweenUtility.DoShowWindow(_container);
        }
    }
}
