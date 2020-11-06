using UnityEngine;
using System.Collections.Generic;

namespace JigiJumper.Spawner
{
    public class PoolSystem<T> where T : UnityEngine.Component
    {
        Stack<T> inactive;

        T prefab;

        public PoolSystem(T prefab)
        {
            this.prefab = prefab;

            inactive = new Stack<T>(5);
        }

        public void Preload(Transform parent, int qty)
        {
            T[] components = new T[qty];
            
            for (int i = 0; i < qty; ++i)
            {
                components[i] = Spawn(Vector3.zero, Quaternion.identity, parent);
            }

            // Now despawn them all.
            for (int i = 0; i < qty; ++i)
            {
                Despawn(components[i]);
            }
        }

        public T Spawn(Vector3 pos, Quaternion rot, Transform parent)
        {
            T component;
            if (inactive.Count == 0)
            {
                component = Object.Instantiate<T>(prefab, pos, rot, parent);
            }
            else
            {
                component = inactive.Pop();

                if (component == null)
                {
                    return Spawn(pos, rot, parent);
                }
            }

            component.transform.position = pos;
            component.transform.rotation = rot;
            component.gameObject.SetActive(true);
            return component;
        }

        public void Despawn(T component)
        {
            component.gameObject.SetActive(false);
            inactive.Push(component);
        }
    }
}
