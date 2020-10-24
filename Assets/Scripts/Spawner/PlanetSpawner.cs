using JigiJumper.Actors;
using UnityEngine;


namespace JigiJumper.Spawner
{
    public class PlanetSpawner : MonoBehaviour
    {
        [SerializeField] PlanetController _planetPrefab = null;

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
                oldPlanet.isVisited = false;
                _planetPool.Despawn(oldPlanet.gameObject);
            }

            SpawnNewPlanet();
        }

        private void SpawnNewPlanet()
        {
            var newPlanet = _planetPool.Spawn(_planetPrefab.gameObject, _lastPalnet.transform.position, Quaternion.identity, transform);

            //todo must grab info about each planet and make new pos based on them

            float xPos = Random.Range(-2f, 2f);
            float yPos = Random.Range(5f, 5.5f);

            float newXPos = newPlanet.transform.position.x + xPos;
            float newYPos = newPlanet.transform.position.y + yPos;

            newPlanet.transform.position = new Vector2(newXPos, newYPos);
        }

        private PlanetController SpawnTheFirst()
        {
            return _planetPool.Spawn(_planetPrefab.gameObject, Vector3.zero, Quaternion.identity, transform).GetComponent<PlanetController>();
        }
    }
}