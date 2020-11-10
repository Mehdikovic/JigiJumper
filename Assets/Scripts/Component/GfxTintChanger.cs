using System.Collections.Generic;
using UnityEngine;

using JigiJumper.Utils;
using JigiJumper.Actors;
using JigiJumper.Spawner;


namespace JigiJumper.Component
{

    public class GfxTintChanger : MonoBehaviour
    {
        private const int MAX_RANGE = 1000000;
        
        [SerializeField] Color[] _colors = new Color[2];

        private Queue<Color> _colorQueue = null;
        private int _seed;
        private PlanetSpawner _planetSpawner;
        
        private void Awake()
        {
            _seed = Random.Range(0, MAX_RANGE);
            _colorQueue = new Queue<Color>(Utility.Shuffle(_colors, _seed));
            
            _planetSpawner = GetComponent<PlanetSpawner>();
            _planetSpawner.OnNewPalnetSpawned += OnNewPalnetSpawned;
        }

        private void OnNewPalnetSpawned(PlanetController planetController)
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

            planetController.SetSpriteColor(color);
        }
    }
}