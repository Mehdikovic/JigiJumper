using System;
using UnityEngine;
using UnityEngine.UI;

namespace JigiJumper.UI
{
    public class PopupUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _container = null;
        [SerializeField] private Button _btnBack = null;

        public event Action OnPopupClosed;

        Action _onPopupClosedCallback;

        private void Awake()
        {
            _btnBack.onClick.AddListener(() =>
            {
                _container.gameObject.SetActive(false);
                _onPopupClosedCallback?.Invoke();
                OnPopupClosed?.Invoke();
            });
     
            _container.gameObject.SetActive(false);
        }

        public void ShowPopup(Action onPopupClosed)
        {
            _onPopupClosedCallback = onPopupClosed;
            _container.gameObject.SetActive(true);
            _container.localScale = new Vector3(0, 0, 1);
         
            Utils.DoTweenUtility.DoShowWindow(_container);
        }
    }
}
