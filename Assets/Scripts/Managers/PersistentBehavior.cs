using UnityEngine;

namespace JigiJumper.Managers {
    public abstract class PersistentBehavior : MonoBehaviour {
        static bool _isInit = false;

        void Awake() {
            if (_isInit) { Destroy(gameObject); }
            _isInit = true;
            DontDestroyOnLoad(gameObject);
            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}