using JigiJumper.Spawner;
using System;
using System.Collections;
using UnityEngine;


namespace JigiJumper.Component
{
    public class ParticleDestroyer : MonoBehaviour
    {
        ParticleSystem _particle;

        private void Awake()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        public void SetColor(Color color)
        {
            var configMainModule = _particle.main;
            configMainModule.startColor = color;
        }

        public void RequestToDespawnToPool(PoolSystem<ParticleDestroyer> pool)
        {
            StartCoroutine(Deactive(pool));
        }

        IEnumerator Deactive(PoolSystem<ParticleDestroyer> pool)
        {
            while (true)
            {
                if (_particle.isStopped)
                {
                    pool.Despawn(this);
                }
                yield return null;
            }
        }
    }
}