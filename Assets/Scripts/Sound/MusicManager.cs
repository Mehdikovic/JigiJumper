using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


namespace JigiJumper.Sound
{
    public class MusicManager : MonoBehaviour
    {
        static bool _isInit = false;

        private AudioSource _audioSource = null;
        private WaitForSeconds _wait;
        private Coroutine _playerCo;

        void Awake()
        {
            if (_isInit) { Destroy(gameObject); }
            _isInit = true;

            DontDestroyOnLoad(gameObject);

            _audioSource = GetComponent<AudioSource>();
            _wait = new WaitForSeconds(.5f);
            _audioSource.loop = true;
            _audioSource.time = 8f;

            SceneManager.activeSceneChanged += OnActiveSceneChanged;

        }

        private void OnActiveSceneChanged(Scene prev, Scene next)
        {
            if (next.buildIndex == 0) { return; }

            if (_playerCo != null)
            {
                StopCoroutine(_playerCo);
                _audioSource.DOFade(0, .3f)
                    .onComplete = () =>
                    {
                        _playerCo = StartCoroutine(PlayBgMusic());
                    };
            }
            else
            {
                _playerCo = StartCoroutine(PlayBgMusic());
            }

        }

        IEnumerator PlayBgMusic()
        {
            yield return _wait;
            _audioSource.Play();
            _audioSource.volume = 0;
            _audioSource.DOFade(1, 2f);
        }
    }
}