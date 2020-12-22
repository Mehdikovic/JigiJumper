using UnityEngine;

namespace JigiJumper.Component {
    public class PlanetTimerSfx : MonoBehaviour {
        [SerializeField] private SelfDestructor _destructor = null;
        [SerializeField] private AudioSource _audio = null;

        private void Awake() {
            _destructor.OnActivateTimer += () => {
                _destructor.OnTimerTick += OnTimerTick;
            };

            _destructor.OnDeactivateTimer += () => {
                _destructor.OnTimerTick -= OnTimerTick;
            };
        }

        private void OnTimerTick(int timer) {
            if (timer > 3) { return; }
            _audio.Play();
        }
    }
}