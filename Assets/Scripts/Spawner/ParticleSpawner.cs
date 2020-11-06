using JigiJumper.Component;
using JigiJumper.Managers;
using UnityEngine;


namespace JigiJumper.Spawner
{
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] ParticleDestroyer _particlePrefab = null;
        
        PoolSystem<ParticleDestroyer> _pool;
        
        void Awake()
        {
            GameManager.instance.jumper.OnPlanetReached += OnPlanetReached;
            _pool = new PoolSystem<ParticleDestroyer>(_particlePrefab);
        }

        private void OnPlanetReached(Actors.PlanetController oldPlanet, Actors.PlanetController newPlanet)
        {
            if (oldPlanet != null) { 
                var activeParticle = _pool.Spawn(oldPlanet.transform.position, Quaternion.identity, transform);
                activeParticle.SetColor(oldPlanet.GetSpriteTint());
                activeParticle.RequestToDespawnToPool(_pool);
            }
        }
    }
}