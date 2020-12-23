using UnityEngine;

namespace JigiJumper.Managers {
    public class PersistentBehavior<T> : MonoBehaviour {
        static bool _isInit = false;

        protected void Awake() {
            if (_isInit) { Destroy(gameObject); }
            _isInit = true;
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}