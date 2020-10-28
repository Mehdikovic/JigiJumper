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

        SimplePool _planetPool;

        private void Awake()
        {
            _planetPool = new SimplePool();
            _planetPool.Preload(_planetPrefab.gameObject, transform, 3);

            _currentPlanet = SpawnTheFirst();
            _jumper = GameManager.Instance.jumper;
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
                _planetPool.Despawn(previousPlanet.gameObject);
            }

            _currentPlanet.InvokeOnJumperEnter();
            SpawnNewPlanet();
        }

        private void SpawnNewPlanet()
        {
            GameObject newPlanet = _planetPool.Spawn(_planetPrefab.gameObject, _currentPlanet.transform.position, Quaternion.identity, transform);
            newPlanet.GetComponent<PlanetController>().InvokeOnComponentInitialization();
        }

        private PlanetController SpawnTheFirst()
        {
            return _planetPool.Spawn(_planetPrefab.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<PlanetController>();
        }
    }
}