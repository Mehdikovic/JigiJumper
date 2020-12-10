using UnityEngine;
using DG.Tweening;

namespace JigiJumper.Utils {
    public static class DoTweenUtility {
        public static void DoShowWindow(RectTransform container, TweenCallback onComplete = null) {
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

        public static void DoHideWindow(RectTransform container, TweenCallback onComplete = null) {
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
    }
}