using UnityEngine;


namespace JigiJumper.Actors
{
    public class PlanetController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 50f;

        [SerializeField] private Transform _pivot = null;

        Transform _circuit;

        private void Awake()
        {
            _circuit = _pivot.GetChild(0);
        }

        void LateUpdate()
        {
            HandleRotation();
        }

        private void HandleRotation()
        {
            _pivot.Rotate(Vector3.forward * (Time.deltaTime * _rotationSpeed));
        }

        public Transform GetPivot()
        {
            return _pivot;
        }

        public Transform GetPivotCircuit()
        {
            return _circuit;
        }
    }
}