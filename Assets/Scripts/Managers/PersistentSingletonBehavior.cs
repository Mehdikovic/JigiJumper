using UnityEngine;


namespace JigiJumper.Managers {
    public abstract class PersistentSingletonBehavior<T> : MonoBehaviour where T : MonoBehaviour {
        protected static T _instance;

        public static T instance {
            get {
                if (_instance == null) {
                    var instances = FindObjectsOfType<T>();

                    if (instances == null || instances.Length == 0) {
                        Debug.LogError("An instance of " + typeof(T) +
                           " is needed in the scene, but there is none.");
                    } else if (instances.Length == 1) {
                        _instance = instances[0];
                        DontDestroyOnLoad(_instance.gameObject);
                    } else if (instances.Length > 1) {
                        Debug.LogError("Instances of " + typeof(T) +
                           " are found in the scene, we need only one instance.");
                        _instance = instances[0];
                        DontDestroyOnLoad(_instance.gameObject);
                        for (int i = 1; i < instances.Length; ++i) {
                            Destroy(instances[i].gameObject);
                        }
                    }
                }
                return _instance;
            }
        }

        protected void Awake() {
            var monoInstance = GetComponent<T>();

            if (_instance == null) {
                _instance = monoInstance;
                DontDestroyOnLoad(gameObject);
            } else if (_instance != monoInstance) {
                Debug.LogError("Instances of " + typeof(T) +
                           " are found in the scene, we need only one instance.");
                Destroy(gameObject);
            }

            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}