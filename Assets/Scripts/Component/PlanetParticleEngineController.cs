using JigiJumper.Actors;
using JigiJumper.Managers;
using UnityEngine;


namespace JigiJumper.Component {
    public class PlanetParticleEngineController : MonoBehaviour {
        [SerializeField] ParticleSystem _particleInstance = null;
        [SerializeField] JumperController _jumper = null;

        private void Awake() {
            _jumper.OnJump += OnJumperJump;
            _jumper.OnPlanetReached += OnPlanetReached;
            _jumper.OnRestart += OnJumperRestart;
        }

        private void OnJumperRestart(int remainingLife, RestartMode _) {
            _particleInstance.Stop();
        }

        private void OnPlanetReached(PlanetController arg1, PlanetController arg2) {
            _particleInstance.Stop();
        }

        private void OnJumperJump() {
            _particleInstance.Play();
        }
    }
}