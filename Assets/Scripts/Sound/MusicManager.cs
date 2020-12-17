using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using JigiJumper.Data;


namespace JigiJumper.Sound {

    public class MusicManager : MonoBehaviour {
        static bool _isInit = false;
        const float MUSIC_VOLUME = 0.5f;
        
        [SerializeField] BgMusicData[] _bgMusics = null;

        private AudioSource _audioSource = null;
        private Coroutine _playerCoroutine;
        private int _lastSceneIndex = -1;
        private int _currentSceneIndex = -1;

        void Awake() {
            if (_isInit) { Destroy(gameObject); }
            _isInit = true;
            DontDestroyOnLoad(gameObject);
            
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable() {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void OnDisable() {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        private void OnSceneUnloaded(Scene scene) {
            _lastSceneIndex = scene.buildIndex;
        }

        private void OnActiveSceneChanged(Scene current, Scene next) {
            if (next.buildIndex == 0) { return; } // don't play on loading level
            if (_lastSceneIndex == next.buildIndex) { return; } // don't play when restarting same level again

            _currentSceneIndex = next.buildIndex;
            RequestToPlayBgMusic();
        }

        IEnumerator PlayBgMusicCoroutine(BgMusicInfo clipData) {
            yield return new WaitForSeconds(1f); // allow mixer to be set by SettingsWindowUI

            _audioSource.clip = clipData.clip;
            _audioSource.time = clipData.start;
            _audioSource.volume = 0;
            _audioSource.Play();
            _audioSource.DOFade(MUSIC_VOLUME, 3f);

            yield return new WaitForSeconds(clipData.GetWaitTime());
            RequestToPlayBgMusic();
        }

        private void RequestToPlayBgMusic() {
            if (_bgMusics == null || _bgMusics.Length == 0) { return; }
            if (_bgMusics.Length <= _currentSceneIndex) { return; } // there is no music clip here

            if (_bgMusics[_currentSceneIndex].HasMusic()) {
                PlayMusic(_bgMusics[_currentSceneIndex].Next());
            }
        }

        private void PlayMusic(BgMusicInfo bgMusicInfo) {
            if (_playerCoroutine != null) {
                StopCoroutine(_playerCoroutine);
                _audioSource
                    .DOFade(0, 0.6f)
                    .onComplete = () => {
                        _audioSource.Stop();
                        _playerCoroutine = StartCoroutine(PlayBgMusicCoroutine(bgMusicInfo));
                    };
            } else {
                _playerCoroutine = StartCoroutine(PlayBgMusicCoroutine(bgMusicInfo));
            }
        }
    }
}