using JigiJumper.Component;
using UnityEngine;


namespace JigiJumper.Spawner
{
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] ParticleDestroyer _particlePrefab = null;
        [SerializeField] PlanetSpawner _planetSpawner = null;

        PoolSystem<ParticleDestroyer> _pool;

        Transform _transform;

        void Awake()
        {
            _transform = transform;
            _pool = new PoolSystem<ParticleDestroyer>(_particlePrefab);
        }

        private void OnEnable()
        {
            _planetSpawner.OnOldPlanetDespawned += OnOldPlanetDespawned;
        }

        private void OnDisable()
        {
            _planetSpawner.OnOldPlanetDespawned -= OnOldPlanetDespawned;
        }

        private void OnOldPlanetDespawned(Actors.PlanetController oldPlanet)
        {
            var activeParticle = _pool.Spawn(oldPlanet.transform.position, Quaternion.identity, _transform);
            activeParticle.SetColor(oldPlanet.GetSpriteTint());
            activeParticle.RequestToDespawnToPool(_pool);
        }
    }
}