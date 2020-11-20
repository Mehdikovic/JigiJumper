using UnityEngine;


namespace JigiJumper.Spawner
{
    public class PersistantGameObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _persistantGameObjectPrefab = null;

        static private bool _isSpawned = false;

        void Awake()
        {
            if (_isSpawned) { return; }

            _isSpawned = true;

            SpawnGameObject();
        }

        void SpawnGameObject()
        {
            var go = Instantiate(_persistantGameObjectPrefab, Vector3.zero, Quaternion.identity);
            DontDestroyOnLoad(go);
        }
    }
}