using JigiJumper.Actors;
using JigiJumper.Data;
using UnityEngine;


namespace JigiJumper.Spawner
{
    public class PlanetSpawner : MonoBehaviour
    {
        //todo delete this line
        [SerializeField] private PlanetType planetType = PlanetType.Medium;
        [SerializeField] PlanetController _planetPrefab = null;
        [SerializeField] PlanetData _planetData = null;

        PlanetController _lastPalnet = null;

        SimplePool _planetPool;

        private void Awake()
        {
            _planetPool = new SimplePool();
            _planetPool.Preload(_planetPrefab.gameObject, transform, 5);

            _lastPalnet = SpawnTheFirst();

            JumperController.OnPlanetReached += OnPlanetReached;
        }

        private void OnPlanetReached(PlanetController oldPlanet, PlanetController newPlanetReached)
        {
            _lastPalnet = newPlanetReached;

            if (oldPlanet != null)
            {
                // todo enable animations
                // reseting planets
                oldPlanet.ResetPlanet();
                _planetPool.Despawn(oldPlanet.gameObject);
            }

            SpawnNewPlanet();
        }

        private void SpawnNewPlanet()
        {
            GameObject newPlanet = _planetPool.Spawn(_planetPrefab.gameObject, _lastPalnet.transform.position, Quaternion.identity, transform);
            PlanetDataStructure data = _planetData.GetPlanetData(planetType);

            newPlanet.GetComponent<PlanetController>().Config(data);
        }

        private PlanetController SpawnTheFirst()
        {
            return _planetPool.Spawn(_planetPrefab.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<PlanetController>();
        }
    }
}