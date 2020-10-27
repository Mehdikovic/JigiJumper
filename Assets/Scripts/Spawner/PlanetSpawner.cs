using JigiJumper.Actors;
using UnityEngine;


namespace JigiJumper.Spawner
{
    public class PlanetSpawner : MonoBehaviour
    {
        [SerializeField] PlanetController _planetPrefab = null;

        PlanetController _lastPalnet;
        JumperController _jumper;

        SimplePool _planetPool;

        private void Awake()
        {
            _planetPool = new SimplePool();
            _planetPool.Preload(_planetPrefab.gameObject, transform, 3);

            _lastPalnet = SpawnTheFirst();
            _jumper = FindObjectOfType<JumperController>();
        }

        private void OnEnable()
        {
            _jumper.OnPlanetReached += OnPlanetReached;
        }

        private void OnDisable()
        {
            _jumper.OnPlanetReached -= OnPlanetReached;
        }

        private void OnPlanetReached(PlanetController oldPlanet, PlanetController newPlanetReached)
        {
            _lastPalnet = newPlanetReached;

            if (oldPlanet != null)
            {
                oldPlanet.OnJumperExit();
                _planetPool.Despawn(oldPlanet.gameObject);
            }

            _lastPalnet.OnJumperEnter();
            SpawnNewPlanet();
        }

        private void SpawnNewPlanet()
        {
            GameObject newPlanet = _planetPool.Spawn(_planetPrefab.gameObject, _lastPalnet.transform.position, Quaternion.identity, transform);
            newPlanet.GetComponent<PlanetController>().InitialComponents(_jumper);
        }

        private PlanetController SpawnTheFirst()
        {
            return _planetPool.Spawn(_planetPrefab.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<PlanetController>();
        }
    }
}