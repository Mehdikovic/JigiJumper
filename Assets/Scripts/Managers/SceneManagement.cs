using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

namespace JigiJumper.Managers
{
    public class SceneManagement : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas = null;
        [SerializeField] private TextMeshProUGUI _text = null;

        public void LoadSceneAsyncAfter(float after, int sceneIndex)
        {
            DOTween.PauseAll();
            StartCoroutine(LoadScene(after, sceneIndex));
        }

        IEnumerator LoadScene(float after, int sceneIndex)
        {
            if (after != 0f)
            {
                yield return new WaitForSeconds(after);
            }
            _canvas.SetActive(true);
            var co = StartCoroutine(AnimateText(.3f));
            yield return SceneManager.LoadSceneAsync(sceneIndex);
            _canvas.SetActive(false);
            StopCoroutine(co);
        }

        IEnumerator AnimateText(float duration)
        {
            var wait = new WaitForSeconds(duration);
            string[] strings = new string[]
            {
                "",
                ".",
                "..",
                "...",
                "....",
            };

            int index = 0;

            while (true)
            {
                index = index % strings.Length;
                _text.text = strings[index++];
                yield return wait;
            }
        }
    }
}