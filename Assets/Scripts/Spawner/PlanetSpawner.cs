using UnityEngine;

using JigiJumper.Managers;
using JigiJumper.Actors;


namespace JigiJumper.Spawner
{
    public class PlanetSpawner : MonoBehaviour
    {
        [SerializeField] PlanetController _planetPrefab = null;

        PlanetController _currentPlanet;
        JumperController _jumper;

        PoolSystem<PlanetController> _planetPool;

        private void Awake()
        {
            _planetPool = new PoolSystem<PlanetController>(_planetPrefab);
            _currentPlanet = SpawnTheFirst();
            _jumper = GameManager.instance.jumper;
        }

        private void OnEnable()
        {
            _jumper.OnPlanetReached += OnPlanetReached;
        }

        private void OnDisable()
        {
            _jumper.OnPlanetReached -= OnPlanetReached;
        }

        private void OnPlanetReached(PlanetController previousPlanet, PlanetController newPlanet)
        {
            _currentPlanet = newPlanet;

            if (previousPlanet != null)
            {
                previousPlanet.InvokeOnPlanetDespawned();
                _planetPool.Despawn(previousPlanet);
            }

            _currentPlanet.InvokeOnJumperEnter();
            SpawnNewPlanet();
        }

        private void SpawnNewPlanet()
        {
            PlanetController newPlanet = _planetPool.Spawn(_currentPlanet.transform.position, Quaternion.identity, transform);
            newPlanet.InvokeOnComponentInitialization();
        }

        private PlanetController SpawnTheFirst()
        {
            return _planetPool.Spawn(Vector3.zero, Quaternion.identity, transform);
        }
    }
}