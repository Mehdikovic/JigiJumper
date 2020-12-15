using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JigiJumper.Ui {
    public static class Ui {
        public static void DoShowWindow(RectTransform container, Action onComplete = null) {
            container.gameObject.SetActive(true);
            container.localScale = new Vector3(0, 0, 1);

            container
                .DOScaleX(1.3f, .3f)
                .onComplete = () => container.DOScaleX(1f, .1f);

            container
                .DOScaleY(1.3f, .3f)
                .onComplete = () => container.DOScaleY(1f, .1f)
                .onComplete = () => onComplete?.Invoke();
        }

        public static void DoHideWindow(RectTransform container, Action onComplete = null) {
            container.localScale = new Vector3(1, 1, 1);

            container
                .DOScaleX(1.2f, .1f)
                .onComplete = () => container.DOScaleX(0f, .3f);

            container
                .DOScaleY(1.2f, .1f)
                .onComplete = () => container.DOScaleY(0f, .3f)
                .onComplete = () => {
                    onComplete?.Invoke();
                    container.localScale = new Vector3(0, 0, 1);
                    container.gameObject.SetActive(false);
                };
        }

        static public void SetBehaviorActivation(bool active, IEnumerable<Behaviour> behaviors) {
            if (behaviors == null) return;

            foreach (var behavoir in behaviors) {
                behavoir.enabled = active;
            }
        }

        static public void TransitionTo(WindowUi from, WindowUi to) {
            from.BeginToHide();
            SetBehaviorActivation(false, from.GetUiBehaviors());

            RectTransform fromRect = from.GetRectWindow();
            RectTransform toRect = to.GetRectWindow();

            DoHideWindow(fromRect,
                onComplete: () => {
                    from.EndOfHide();
                    to.BeginToShow();
                    DoShowWindow(toRect,
                        onComplete: () => {
                            SetBehaviorActivation(true, to.GetUiBehaviors());
                            to.EndOfShow();
                        }
                    );
                }
            );
        }
    }
}