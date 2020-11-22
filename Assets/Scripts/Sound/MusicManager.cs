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
            _wait = new WaitForSeconds(1f);
            _audioSource.loop = true;
            _audioSource.time = 15f;

            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void OnActiveSceneChanged(Scene prev, Scene next)
        {
            if (next.buildIndex == 0) { return; }

            if (_playerCo != null)
            {
                StopCoroutine(_playerCo);
                _audioSource.DOFade(0, .6f)
                    .onComplete = () =>
                    {
                        _audioSource.Stop();
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
            yield return _wait; // lets allow mixer to be set by SettingsWindowUI
            _audioSource.Play();
            _audioSource.volume = 0;
            _audioSource.DOFade(1, 3f);

            while (_audioSource.isPlaying)
            {
                float passedTimePercent = _audioSource.time / _audioSource.clip.length;
                if (passedTimePercent <= .95f)
                {
                    //Do Play next music
                }

                yield return _wait;
            }
        }
    }
}