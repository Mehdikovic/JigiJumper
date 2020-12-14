using UnityEngine;


namespace JigiJumper.Managers {
    public abstract class SingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour {
        protected static T _instance;

        public static T instance {
            get {
                if (_instance == null) {
                    //_instance = (T)FindObjectOfType(typeof(T));
                    _instance = FindObjectOfType<T>();

                    if (_instance == null) {
                        Debug.LogError("An instance of " + typeof(T) +
                           " is needed in the scene, but there is none.");
                    }
                }
                return _instance;
            }
        }

        void Awake() {
            var monoInstance = GetComponent<T>();

            if (_instance == null) {
                _instance = monoInstance;
            } else if (_instance != monoInstance) {
                Destroy(_instance.gameObject);
                _instance = monoInstance;
            }
            OnAwake();
        }
        protected virtual void OnAwake() { }
    }
}