using JigiJumper.Actors;
using System.Collections;
using UnityEngine;


namespace JigiJumper.Component
{
    public class TrailController : MonoBehaviour
    {
        private TrailRenderer _trail;
        private JumperController _jumper;

        private void Awake()
        {
            _trail = GetComponent<TrailRenderer>();
            _jumper = GetComponent<JumperController>();
            
            _jumper.OnRestart += OnRestartDemand;
            _jumper.OnPlanetReached += OnPlanetReached;
            _jumper.OnJump += OnJumperJump;
        }

        private void OnJumperJump()
        {
            _trail.enabled = true;
        }

        private void OnPlanetReached(PlanetController arg1, PlanetController arg2)
        {
            OnRestartDemand();
        }

        private void OnRestartDemand()
        {
            _trail.enabled = false;
        }
    }
}