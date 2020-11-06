using JigiJumper.Actors;
using JigiJumper.Managers;
using UnityEngine;


namespace JigiJumper.Manager
{
    public class RestartManager : MonoBehaviour
    {
        private const float RESTART_TIMER = 2f;

        private JumperController _jumper;
        private float _restartTimerChecker;
        private bool _isRestartCalled = true;
        
        private void Awake()
        {
            _jumper = GetComponent<GameManager>().jumper;
            _jumper.OnJump += OnJumperJump;
        }

        private void Update()
        {
            if (_jumper.currentPlanetGameObject != null) { return; }
            if (_isRestartCalled) { return; }

            _restartTimerChecker += Time.deltaTime;

            if (_restartTimerChecker >= RESTART_TIMER)
            {
                _isRestartCalled = true;
                RequestToRestartTheGame();
            }
        }

        private void RequestToRestartTheGame()
        {
            // todo show interactive ui to user for choosing an action
        }

        private void OnJumperJump()
        {
            _restartTimerChecker = 0;
            _isRestartCalled = false;
        }
    }
}