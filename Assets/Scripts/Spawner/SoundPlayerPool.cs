using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JigiJumper.Spawner {
    public class SoundPlayerPool : MonoBehaviour {
        static bool _isInit = false;

        [SerializeField] AudioSource _audioPrefab = null;
        PoolSystem<AudioSource> _pool;
        Dictionary<int, AudioSource> _audiosMap = null;
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

            _audiosMap = new Dictionary<int, AudioSource>();
        }

        public void PlayMusic(AudioClip clip) {
            AudioSource audio = _pool.Spawn(Vector3.zero, Quaternion.identity, _transform);
            audio.clip = clip;
            audio.Play();
            StartCoroutine(DespawnAudioSource(audio, clip.length));
        }

        public int ManualPlayMusic(AudioClip clip) {
            AudioSource audio = _pool.Spawn(Vector3.zero, Quaternion.identity, _transform);
            _audiosMap.Add(audio.GetInstanceID(), audio);
            audio.clip = clip;
            audio.Play();
            return audio.GetInstanceID();
        }

        public void ManualDespawn(int id) {
            var audio = _audiosMap[id];
            _audiosMap.Remove(id);
            audio.Stop();
            _pool.Despawn(audio);
        }

        IEnumerator DespawnAudioSource(AudioSource audio, float time) {
            yield return new WaitForSeconds(time);
            audio.Stop();
            _pool.Despawn(audio);
        }
    }
}