using UnityEngine;

using JigiJumper.Data;
using JigiJumper.Actors;
using JigiJumper.Managers;


namespace JigiJumper.Component
{
    public class SelfDestructor : MonoBehaviour, IPlanetEventHandler
    {
        [Range(5f, 10f)]
        [SerializeField] private float _selfDestructionTimer = 5f;

        private float _timer;
        private JumperController _jumper;
        private bool _isActivated;
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        public bool isActiveComponent => _isActivated;
        public float timer => _timer;

        //called before the JumperEnter and wouldn't be called for the first spawned object
        public void OnInitialDataReceived(JumperController jumper, PlanetDataStructure data)
        {
            _timer = _selfDestructionTimer;
            _jumper = jumper;
            
        }

        private void Update()
        {
            if (_isActivated == false) { return; }

            _timer -= Time.deltaTime;
            if (_timer <= 0f)
            {
                _timer = 0f;
                _gameManager.RequestSelfDestructionPlanet(gameObject);
                _isActivated = false;
            }
        }

        public void OnJumperEnter()
        {
            // if jumper is null then i'm the first spawned planet
            // and don't need to be selfdestruction
            if (_jumper == null) { return; } 
            
            _isActivated = true; // todo read this from probability
        }

        public void OnJumperExit()
        {
            _isActivated = false;
        }
    }
}