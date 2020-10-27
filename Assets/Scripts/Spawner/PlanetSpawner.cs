using JigiJumper.Actors;
using JigiJumper.Managers;
using UnityEngine;


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
                previousPlanet.OnDespawningPreviousPlanet();
                _planetPool.Despawn(previousPlanet.gameObject);
            }

            _currentPlanet.OnJumperEnter();
            SpawnNewPlanet();
        }

        private void SpawnNewPlanet()
        {
            GameObject newPlanet = _planetPool.Spawn(_planetPrefab.gameObject, _currentPlanet.transform.position, Quaternion.identity, transform);
            newPlanet.GetComponent<PlanetController>().InitialComponents();
        }

        private PlanetController SpawnTheFirst()
        {
            return _planetPool.Spawn(_planetPrefab.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<PlanetController>();
        }
    }
}