using JigiJumper.Managers;
using UnityEngine;


namespace JigiJumper.Spawner
{
    public class ParticleSpawner : MonoBehaviour
    {

        [SerializeField] ParticleSystem _particlePrefab = null;
        
        void Awake()
        {
            GameManager.Instance.jumper.OnPlanetReached += OnPlanetReached;
        }

        private void OnPlanetReached(Actors.PlanetController oldPlanet, Actors.PlanetController newPlanet)
        {
            if (oldPlanet != null)
                Instantiate(_particlePrefab, oldPlanet.transform.position, Quaternion.identity);
        }
    }
}