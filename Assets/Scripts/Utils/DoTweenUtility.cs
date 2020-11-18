using UnityEngine;
using DG.Tweening;

namespace JigiJumper.Utils
{
    public static class DoTweenUtility
    {
        public static void DoShowWindow(RectTransform container, TweenCallback onComplete = null)
        {
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
    }
}