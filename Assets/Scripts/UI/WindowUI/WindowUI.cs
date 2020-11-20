using JigiJumper.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace JigiJumper.UI
{
    public abstract class WindowUI : MonoBehaviour
    {

        [SerializeField] protected RectTransform _selfRect = null;
        [Header("Settings")]
        [SerializeField] protected SettingData _setting = null;

        public IEnumerable<Behaviour> GetUIBehaviors() => _behaviorUIs;
        public RectTransform GetRect() => _selfRect;

        protected Behaviour[] _behaviorUIs;

        protected virtual void BeginToHide() { }
        protected virtual void EndOfHide() { }

        protected virtual void BeginToShow() { }
        protected virtual void EndOfShow() { }

        static public void SetActivation(bool active, IEnumerable<Behaviour> behaviors)
        {
            if (behaviors == null) return;

            foreach (var behavoir in behaviors)
            {
                behavoir.enabled = active;
            }
        }

        static public void TransitionToWindow(WindowUI from, WindowUI to)
        {
            from.BeginToHide();
            SetActivation(false, from.GetUIBehaviors());

            RectTransform fromRect = from.GetRect();
            RectTransform toRect = to.GetRect();

            Utils.DoTweenUtility.DoHideWindow(fromRect,
                onComplete: () =>
                {
                    from.EndOfHide();
                    to.BeginToShow();
                    Utils.DoTweenUtility.DoShowWindow(toRect,
                        onComplete: () =>
                        {
                            SetActivation(true, to.GetUIBehaviors());
                            to.EndOfShow();
                        });
                });
        }
    }
}