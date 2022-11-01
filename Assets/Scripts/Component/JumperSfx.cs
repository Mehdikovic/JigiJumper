using JigiJumper.Actors;
using JigiJumper.Managers;
using JigiJumper.Spawner;
using UnityEngine;

namespace JigiJumper.Component {
    [RequireComponent(typeof(AudioSource))]
    public class JumperSfx : MonoBehaviour {
        [SerializeField] JumperController _jumper = null;
        [SerializeField] AudioClip _landClip = null;
        [SerializeField] AudioClip _jumpClip = null;
        [SerializeField] AudioClip _afterDeathClip = null;

        SoundPlayerPool _soundPool;

        private void Awake() {
            _soundPool = FindObjectOfType<SoundPlayerPool>();
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
            _soundPool.PlayMusic(_landClip);
        }

        private void JumperOnJump() {
            _soundPool.PlayMusic(_jumpClip);
        }

        private void JumperOnRestart(int remainingLife, RestartMode mode) {
            if (mode == RestartMode.AfterAdWatched) { return; }
            _soundPool.PlayMusic(_afterDeathClip);
        }
    }
}