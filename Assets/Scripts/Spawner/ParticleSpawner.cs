using JigiJumper.Component;
using System.Collections;
using UnityEngine;

using static JigiJumper.Utils.Utility;

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

        private IEnumerator Start()
        {
            var camera = Camera.main;
            yield return new WaitForSeconds(2f);

            while (true)
            {
                float scale = (0.1f) * Random.Range(1f, 4f);
                SpawnParticle(
                    GetRandomPosOnScreen(camera),
                    new Vector3(scale, scale, 1f),
                    Random.ColorHSV(0.0f, 1.0f, 0.8f, 0.9f, 1f, 1f));
                yield return new WaitForSeconds(Random.Range(.5f, 1f));
            }
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
            SpawnParticle(oldPlanet.transform.position, Vector3.one, oldPlanet.GetSpriteTint());
        }

        private void SpawnParticle(Vector3 position, Vector3 scale, Color color)
        {
            var activeParticle = _pool.Spawn(position, Quaternion.identity, _transform);
            activeParticle.transform.localScale = scale;
            activeParticle.SetColor(color);
            activeParticle.RequestToDespawnToPool(_pool);
        }
    }
}