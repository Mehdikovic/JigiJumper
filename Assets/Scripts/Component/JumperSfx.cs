using JigiJumper.Actors;
using JigiJumper.Spawner;
using UnityEngine;

namespace JigiJumper.Component {
    [RequireComponent(typeof(AudioSource))]
    public class JumperSfx : MonoBehaviour {
        [SerializeField] JumperController _jumper = null;
        [SerializeField] AudioClip _clip = null;

        SoundPlayerPool _soundPool;
        AudioSource _audioSource;

        private void Awake() {
            _soundPool = FindObjectOfType<SoundPlayerPool>();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = false;
            _jumper.OnRestart += JumperOnRestart;
        }

        private void OnEnable() {
            _jumper.OnPlanetReached += JumperOnPlanetReached;
            _jumper.OnJump += JumperOnJump;
        }

        
        private void OnDisable() {
            _jumper.OnPlanetReached -= JumperOnPlanetReached;
            _jumper.OnJump -= JumperOnJump;
        }

        private void JumperOnPlanetReached(PlanetController arg1, PlanetController arg2) {
            _soundPool.PlayMusic(_clip);
            //_audioSource.Stop();
        }

        private void JumperOnJump() {
            //_audioSource.Play();
            //_soundPool.PlayMusic(_clip);
        }

        private void JumperOnRestart(int obj) {
        }


    }
}