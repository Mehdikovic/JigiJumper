using JigiJumper.Data;
using System.Collections.Generic;
using UnityEngine;


namespace JigiJumper.UI {
    public abstract class WindowUi : MonoBehaviour {

        [SerializeField] protected RectTransform _selfRectWindow = null;
        [Header("Settings")]
        [SerializeField] protected SettingData _setting = null;

        public IEnumerable<Behaviour> GetUiBehaviors() => _uiBehaviors;

        public RectTransform GetRectWindow() => _selfRectWindow;

        protected Behaviour[] _uiBehaviors;

        protected virtual void BeginToHide() { }

        protected virtual void EndOfHide() { }

        protected virtual void BeginToShow() { }

        protected virtual void EndOfShow() { }

        static public void SetActivation(bool active, IEnumerable<Behaviour> behaviors) {
            if (behaviors == null) return;

            foreach (var behavoir in behaviors) {
                behavoir.enabled = active;
            }
        }

        static public void TransitionToWindow(WindowUi from, WindowUi to) {
            from.BeginToHide();
            SetActivation(false, from.GetUiBehaviors());

            RectTransform fromRect = from.GetRectWindow();
            RectTransform toRect = to.GetRectWindow();

            Utils.DoTweenUtility.DoHideWindow(fromRect,
                onComplete: () => {
                    from.EndOfHide();
                    to.BeginToShow();
                    Utils.DoTweenUtility.DoShowWindow(toRect,
                        onComplete: () => {
                            SetActivation(true, to.GetUiBehaviors());
                            to.EndOfShow();
                        }
                    );
                }
            );
        }
    }
}