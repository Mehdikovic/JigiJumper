using JigiJumper.Actors;
using System.Collections;
using UnityEngine;


namespace JigiJumper.Component
{
    public class TrailController : MonoBehaviour
    {
        private TrailRenderer _trail;

        private Coroutine _activation;
        WaitForSeconds _waitForOneSeconds;

        private void Awake()
        {
            _waitForOneSeconds = new WaitForSeconds(1f);
            _trail = GetComponent<TrailRenderer>();
            GetComponent<JumperController>().OnRestart += OnRestartDemand;
            GetComponent<JumperController>().OnPlanetReached += OnPlanetReached;
        }

        private void OnPlanetReached(PlanetController arg1, PlanetController arg2)
        {
            OnRestartDemand();
        }

        private void OnRestartDemand()
        {
            _trail.enabled = false;

            if (_activation != null)
            {
                StopCoroutine(_activation);
                _activation = null;
            }

            _activation = StartCoroutine(Active());
        }

        IEnumerator Active()
        {
            yield return _waitForOneSeconds;
            _trail.enabled = true;
        }
    }
}