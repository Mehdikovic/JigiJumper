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
                oldPlanet.isVisited = false;
                _planetPool.Despawn(oldPlanet.gameObject);
            }

            SpawnNewPlanet();
        }

        private void SpawnNewPlanet()
        {
            GameObject newPlanet = _planetPool.Spawn(_planetPrefab.gameObject, _lastPalnet.transform.position, Quaternion.identity, transform);
           
            //todo must grab info about each planet and make new pos based on them
            PlanetDataStructure data = _planetData.GetPlanetData(planetType);

            Vector3 scale = newPlanet.transform.localScale;
            newPlanet.GetComponent<PlanetController>().SetCircuitRadius(data.curcuitPosY);

            newPlanet.transform.localScale = new Vector3(data.radius, data.radius, scale.z);

            float xPos = Random.Range(data.xRange.x, data.xRange.y);
            float yPos = Random.Range(data.yRange.x, data.yRange.y);

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