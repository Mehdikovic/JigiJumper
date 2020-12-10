using UnityEngine;
using JigiJumper.NPC;
using System.Collections;

using static JigiJumper.Utils.Utility;

namespace JigiJumper.Spawner {
    public class NPCJumperSpanwer : MonoBehaviour {
        [SerializeField] private NPCJumper _npcJumperPrefab = null;

        Camera _camera;
        PoolSystem<NPCJumper> _pool;
        Transform _transform;

        private void Awake() {
            _camera = Camera.main;
            _pool = new PoolSystem<NPCJumper>(_npcJumperPrefab);
            _transform = transform;
        }

        private IEnumerator Start() {
            yield return new WaitForSeconds(2);

            while (true) {
                SpawnTheJumper();
                yield return new WaitForSeconds(Random.Range(.3f, 4f));
            }
        }

        private void SpawnTheJumper() {
            Vector2 start = GetRandomPosOnScreen(_camera);
            Vector2 end = GetRandomPosOnScreen(_camera);

            Vector2 dir = (end - start).normalized;

            start += -dir * 10;

            var npc = _pool.Spawn(start, Quaternion.identity, _transform);
            npc.Init(_pool);
            npc.Rotate(dir);
        }
    }
}