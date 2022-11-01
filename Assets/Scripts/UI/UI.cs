using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JigiJumper.Ui {
    public static class UI {
        public static void DoShowWindow(RectTransform container, Action onComplete = null) {
            container.gameObject.SetActive(true);
            container.localScale = new Vector3(0, 0, 1);

            Sequence initial = DOTween.Sequence();
            initial.onComplete = () => {
                Sequence second = DOTween.Sequence();
                second.onComplete = () => onComplete?.Invoke();
                second.Join(container.DOScaleX(1f, .1f));
                second.Join(container.DOScaleY(1f, .1f));
            };
            initial.Join(container.DOScaleX(1.3f, 0.3f));
            initial.Join(container.DOScaleY(1.3f, 0.3f));
        }

        public static void DoHideWindow(RectTransform container, Action onComplete = null) {
            container.localScale = new Vector3(1, 1, 1);

            Sequence initial = DOTween.Sequence();
            initial.onComplete = () => {
                Sequence second = DOTween.Sequence();
                second.onComplete = () => {
                    onComplete?.Invoke();
                    container.localScale = new Vector3(0, 0, 1);
                    container.gameObject.SetActive(false);
                };
                second.Join(container.DOScaleX(0f, .3f));
                second.Join(container.DOScaleY(0f, .3f));
            };
            initial.Join(container.DOScaleX(1.2f, 0.1f));
            initial.Join(container.DOScaleY(1.2f, 0.1f));
        }

        static public void SetBehaviorActivation(bool active, IEnumerable<Behaviour> behaviors) {
            if (behaviors == null) return;

            foreach (var behavoir in behaviors) {
                behavoir.enabled = active;
            }
        }

        static public void TransitionTo(WindowUI from, WindowUI to) {
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