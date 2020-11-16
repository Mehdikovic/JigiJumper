using JigiJumper.Actors;
using JigiJumper.Managers;
using UnityEngine;


namespace JigiJumper.Component
{
    public class Restarter : MonoBehaviour
    {
        private const float RESTART_TIMER = 1.5f;

        private JumperController _jumper;
        private GameManager _gameManager;
        private float _restartTimerChecker;
        private bool _isRestartCalled = true;
        
        private void Awake()
        {
            _gameManager = GameManager.instance;
            _jumper = GetComponent<JumperController>();
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
            _gameManager.RequestToRestart(RestartMode.Reallocate, 0);
        }

        private void OnJumperJump()
        {
            _restartTimerChecker = 0;
            _isRestartCalled = false;
        }
    }
}