using JigiJumper.Spawner;
using UnityEngine;


namespace JigiJumper.NPC
{
    public class NPCJumper : MonoBehaviour
    {
        private Transform _transform;
        private float _speed;
        private PoolSystem<NPCJumper> _pool;
        private float _timer = 0f;

        void Awake()
        {
            _transform = transform;
        }

        void Update()
        {
            _transform.Translate(Vector2.up * (Time.deltaTime * _speed));

            _timer += Time.deltaTime;

            if (_timer >= 7f)
            {
                _timer = 0f;
                _pool.Despawn(this);
            }
        }

        public void Init(PoolSystem<NPCJumper> pool)
        {
            _pool = pool;
        }

        public void Rotate(Vector2 dir)
        {
            _timer = 0;
            _speed = Random.Range(4, 21);
            _transform.localRotation = Quaternion.identity;
            float angle = Vector2.SignedAngle(_transform.up, dir);
            _transform.localRotation *= Quaternion.Euler(Vector3.forward * angle);
        }
    }
}