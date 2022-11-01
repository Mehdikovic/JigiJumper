using JigiJumper.Actors;
using UnityEngine;


namespace JigiJumper.Component {
    public class JumperSpriteController : MonoBehaviour {
        [SerializeField] private JumperController _jumper = null;
        [SerializeField] private ParticleSystem _particle = null;

        Transform _transform;
        
        private void Awake() {
            _transform = transform;
            _jumper.OnPlanetReached += OnJumperPlanetReached;
        }

        private void OnJumperPlanetReached(PlanetController oldPlanet, PlanetController newPlanet) {
            switch (newPlanet.PlanetType) {
                case Data.PlanetType.Large:
                case Data.PlanetType.Medium:
                    _transform.localScale = new Vector3(.5f, .5f, 1);
                    break;
                case Data.PlanetType.Small:
                    _transform.localScale = new Vector3(.4f, .4f, 1);
                    break;
                case Data.PlanetType.Tiny:
                    _transform.localScale = new Vector3(.25f, .25f, 1);
                    break;
            }

            EmitParticle();
        }

        private void EmitParticle() {
            if (_particle.isPlaying)
                _particle.Stop();
            _particle.Play();
        }
    }
}