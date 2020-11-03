using JigiJumper.Actors;
using JigiJumper.Data;
using JigiJumper.Utils;
using System.Collections.Generic;
using UnityEngine;


namespace JigiJumper.Component
{

    public class GfxTintChanger : MonoBehaviour
    {
        private const int MAX_RANGE = 1000000;
        
        [SerializeField] SpriteRenderer _spriteRenderer = null;
        [SerializeField] Color[] _colors = new Color[2];

        private Queue<Color> _colorQueue = null;
        private int _seed;
        private PlanetController _planetController;

        private void Awake()
        {
            _seed = Random.Range(0, MAX_RANGE);
            _colorQueue = new Queue<Color>(Utility.Shuffle(_colors, _seed));
            _planetController = GetComponent<PlanetController>();

            _planetController.OnSpawnedInitialization += ChangeColor;
        }

        void ChangeColor(PlanetDataStructure data)
        {
           
            Color color;
            if (_colorQueue.Count > 0)
            {
                color = _colorQueue.Dequeue();
            }
            else
            {
                _colorQueue = new Queue<Color>(Utility.Shuffle(_colors, ++_seed % MAX_RANGE));
                color = _colorQueue.Dequeue();
            }

            _spriteRenderer.color = color;
        }

    }
}