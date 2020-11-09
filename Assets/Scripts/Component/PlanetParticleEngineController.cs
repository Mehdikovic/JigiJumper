using JigiJumper.Actors;
using UnityEngine;


namespace JigiJumper.Component
{
    public class PlanetParticleEngineController : MonoBehaviour
    {
        [SerializeField] ParticleSystem _particleInstance = null;
        [SerializeField] JumperController _jumper = null;

        private void Awake()
        {
            _jumper.OnJump += OnJumperJump;
            _jumper.OnPlanetReached += OnPlanetReached;
            _jumper.OnRestart += OnJumperRestart;
        }

        private void OnJumperRestart()
        {
            _particleInstance.Stop();
        }

        private void OnPlanetReached(PlanetController arg1, PlanetController arg2)
        {
            _particleInstance.Stop();
        }

        private void OnJumperJump()
        {
            _particleInstance.Play();
        }
    }
}