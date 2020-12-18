using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JigiJumper.Spawner {
    public class SoundPlayerPool : MonoBehaviour {
        static bool _isInit = false;

        [SerializeField] AudioSource _audioPrefab = null;
        PoolSystem<AudioSource> _pool;
        Transform _transform;

        void Awake() {
            if (_isInit) { Destroy(gameObject); }
            _isInit = true;
            DontDestroyOnLoad(gameObject);
            
            _transform = transform;

            var preAudioSources = GetComponentsInChildren<AudioSource>();
            if (preAudioSources != null && preAudioSources.Length > 0) {
                _pool = new PoolSystem<AudioSource>(_audioPrefab, preAudioSources);
            } else {
                _pool = new PoolSystem<AudioSource>(_audioPrefab);
            }
        }

        public void PlayMusic(AudioClip clip) {
            AudioSource audio = _pool.Spawn(Vector3.zero, Quaternion.identity, _transform);
            audio.clip = clip;
            audio.Play();
            StartCoroutine(DespawnAudioSource(audio, clip.length));
        }

        public AudioSource ManualPlayMusic(AudioClip clip) {
            AudioSource audio = _pool.Spawn(Vector3.zero, Quaternion.identity, _transform);
            audio.clip = clip;
            audio.Play();
            return audio;
        }

        public void ManualDespawn(AudioSource audio) {
            audio.Stop();
            _pool.Despawn(audio);
        }

        public void StopAllAudios(AudioSource[] audios) {
            if (audios == null || audios.Length < 1) { return; }

            foreach (var audio in audios) {
                if (audio == null) continue;
                audio.Stop();
                ManualDespawn(audio);
            }
        }

        IEnumerator DespawnAudioSource(AudioSource audio, float time) {
            yield return new WaitForSeconds(time);
            audio.Stop();
            _pool.Despawn(audio);
        }
    }
}