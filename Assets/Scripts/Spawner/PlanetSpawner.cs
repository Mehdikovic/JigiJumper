using JigiJumper.Actors;
using UnityEngine;


namespace JigiJumper.Spawner
{
    public class PlanetSpawner : MonoBehaviour
    {
        [SerializeField] PlanetController _largePlanetPrefab = null;
        [SerializeField] PlanetController _mediumPlanetPrefab = null;

        PlanetController _lastPalnet = null;

        SimplePool _largePool;
        SimplePool _mediumPool;

        private void Awake()
        {
            _largePool = new SimplePool();
            _largePool.Preload(_largePlanetPrefab.gameObject, transform, 3);

            _lastPalnet = SpawnTheFirst();

            _mediumPool = new SimplePool();
            _mediumPool.Preload(_mediumPlanetPrefab.gameObject, transform, 5);

            JumperController.OnPlanetReached += OnPlanetReached;
        }

        private void OnPlanetReached(PlanetController oldPlanet, PlanetController newPlanetReached)
        {
            _lastPalnet = newPlanetReached;

            if (oldPlanet != null)
            {
                _largePool.Despawn(oldPlanet.gameObject);
            }

            SpawnNewPlanet();
        }

        private void SpawnNewPlanet()
        {
            Vector3 newPos = _lastPalnet.transform.position;
            newPos.y += 5.5f;
            newPos.x += .5f;

            _largePool.Spawn(_largePlanetPrefab.gameObject, newPos, Quaternion.identity, transform);
        }

        private PlanetController SpawnTheFirst()
        {
            return _largePool.Spawn(_largePlanetPrefab.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<PlanetController>();
        }
    }
}