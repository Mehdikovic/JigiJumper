using Cinemachine;
using UnityEngine;

namespace JigiJumper.Actors
{
    public class JumperController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _cinemachine = null;
        [SerializeField] private float _speed = 10f;
        
        PlanetController _planet;

        private void Update()
        {
            HandleInput();

            if (_planet != null)
            {
                transform.position = _planet.GetPivotCircuit().position;
                transform.rotation = _planet.GetPivotCircuit().rotation;
            }
            else
            {
                transform.Translate(Vector2.up * (Time.deltaTime * _speed));
            }
        }

        private void HandleInput()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _planet = null;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PlanetController planetController = collision.GetComponent<PlanetController>();
            
            if (planetController == null) { return; }

            _planet = planetController;

            Transform pivot = _planet.GetPivot();

            float angle = Vector2.SignedAngle(pivot.transform.up, -transform.up);

            pivot.localRotation *= Quaternion.Euler(Vector3.forward * angle);

            _cinemachine.Follow = _planet.transform;
        }
    }
}