using System.Collections;
using UnityEngine;

using JigiJumper.Data;
using JigiJumper.Actors;
using JigiJumper.Managers;


namespace JigiJumper.Component
{
    public class Oscillator : MonoBehaviour
    {
        float _speed = 0f;
        float _lerpValue = 0f;
        PlanetDataStructure _data;

        float _originalX;
        Coroutine _oscillator;

        private void Awake()
        {
            PlanetController planetController = GetComponent<PlanetController>();

            planetController.OnSpawnedInitialization += OnSpawnedInitialization;
            planetController.OnJumperEnter += OnJumperEnter;
        }

        void Init(PlanetDataStructure data)
        {
            _data = data;
            _originalX = transform.position.x;
            transform.localScale = new Vector3(data.radius, data.radius, 1f);

            _lerpValue = Random.value;

            float yPos = Random.Range(data.yRange.x, data.yRange.y);
            float xPos = Mathf.Lerp(data.xRange.x, data.xRange.y, _lerpValue);
            float newYPos = transform.position.y + yPos;
            float newXPos = _originalX + xPos;

            transform.position = new Vector2(newXPos, newYPos);
        }

        void StopOscillattion()
        {
            if (_oscillator == null) { return; }
            StopCoroutine(_oscillator);
            _oscillator = null;
        }

        void InitialOscillattion()
        {
            _oscillator = StartCoroutine(Tick());
        }

        IEnumerator Tick()
        {
            int direction = 1;
            while (true)
            {
                _lerpValue += Time.deltaTime * direction * _speed;

                if (_lerpValue >= 1f)
                {
                    direction *= -1;
                    _lerpValue = 1f;
                    yield return null;
                }

                if (_lerpValue <= 0f)
                {
                    direction *= -1;
                    _lerpValue = 0f;
                    yield return null;
                }

                float xPos = Mathf.Lerp(_data.xRange.x, _data.xRange.y, _lerpValue);

                float newXPos = _originalX + xPos;
                float newYPos = transform.position.y;

                transform.position = new Vector2(newXPos, newYPos);

                yield return null;
            }
        }

        public void OnSpawnedInitialization(PlanetDataStructure data)
        {
            Init(data);
            var spawnProbabilities = GameManager.Instance.GetSpawnProbabilities();
            _speed = spawnProbabilities.GetOscillationSpeed();
            InitialOscillattion();
        }

        public void OnJumperEnter()
        {
            StopOscillattion();
        }
    }
}