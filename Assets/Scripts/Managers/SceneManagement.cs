using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

namespace JigiJumper.Managers
{
    public class SceneManagement : MonoBehaviour
    {
        static readonly string[] STRING_ANIMATION = new string[] { "", ".", "..", "...", "...."};
        
        [SerializeField] private GameObject _canvas = null;
        [SerializeField] private TextMeshProUGUI _text = null;

        public void LoadSceneAsyncAfter(float after, int sceneIndex)
        {
            DOTween.PauseAll();
            StartCoroutine(LoadScene(after, sceneIndex));
        }

        IEnumerator LoadScene(float after, int sceneIndex)
        {
            var co = StartCoroutine(AnimateText(.3f));

            if (after != 0f)
            {
                yield return new WaitForSeconds(after);
            }
            _canvas.SetActive(true);

            yield return SceneManager.LoadSceneAsync(sceneIndex);
            _canvas.SetActive(false);
            StopCoroutine(co);
        }

        IEnumerator AnimateText(float duration)
        {
            var wait = new WaitForSeconds(duration);
            int index = 0;

            while (true)
            {
                index %= STRING_ANIMATION.Length;
                _text.text = STRING_ANIMATION[index++];
                yield return wait;
            }
        }
    }
}