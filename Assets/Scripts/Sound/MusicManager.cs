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
        private Coroutine _playerCoroutine;
        private int _lastSceneUnloaded = -1;


        void Awake()
        {
            if (_isInit) { Destroy(gameObject); }
            _isInit = true;

            DontDestroyOnLoad(gameObject);

            _audioSource = GetComponent<AudioSource>();
            _wait = new WaitForSeconds(1f);
            _audioSource.time = 15f;

            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void OnSceneUnloaded(Scene scene)
        {
            _lastSceneUnloaded = scene.buildIndex;
        }

        private void OnActiveSceneChanged(Scene current, Scene next)
        {
            if (next.buildIndex == 0) { return; } // don't play on loading level
            if (_lastSceneUnloaded == next.buildIndex) { return; } // don't play when restarting same level again

            PlayMusic();
        }

        private void PlayMusic()
        {
            if (_playerCoroutine != null)
            {
                StopCoroutine(_playerCoroutine);
                _audioSource
                    .DOFade(0, 0.6f)
                    .onComplete = () =>
                    {
                        _audioSource.Stop();
                        _playerCoroutine = StartCoroutine(PlayBgMusicCoroutine());
                    };
            }
            else
            {
                _playerCoroutine = StartCoroutine(PlayBgMusicCoroutine());
            }
        }

        IEnumerator PlayBgMusicCoroutine()
        {
            yield return _wait; // lets allow mixer to be set by SettingsWindowUI
            _audioSource.Play();
            _audioSource.volume = 0;
            _audioSource.DOFade(1, 3f);

            while (_audioSource.isPlaying)
            {
                float passedTimePercent = _audioSource.time / _audioSource.clip.length;
                if (passedTimePercent >= 0.95f)
                {
                    PlayMusic();
                }
                yield return _wait;
            }
        }
    }
}