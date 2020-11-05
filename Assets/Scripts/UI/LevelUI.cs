using JigiJumper.Managers;
using TMPro;
using UnityEngine;


namespace JigiJumper.UI
{
    public class LevelUI : MonoBehaviour
    {
        private const int REMAINING_PLANET = 3;
        
        [SerializeField] private GameObject[] _actives = null;
        [SerializeField] private TextMeshProUGUI _textPro = null;

        int _planetsRemainings = REMAINING_PLANET;

        private void Awake()
        {
            _textPro.text = "1";

            GameManager.Instance.OnLevelChanged += OnLevelChanged;
            GameManager.Instance.jumper.OnPlanetReached += OnPlanetReached;
            
        }

        private void OnPlanetReached(Actors.PlanetController arg1, Actors.PlanetController arg2)
        {
            --_planetsRemainings;

            if (_planetsRemainings <= 0)
            {
                _planetsRemainings = 0;
                SetActiveOfGameObjects(false);
            }
        }

        private void OnLevelChanged(int newLevel)
        {
            _planetsRemainings = REMAINING_PLANET;
            SetActiveOfGameObjects(true);
            _textPro.text = newLevel.ToString();
        }

        private void SetActiveOfGameObjects(bool value)
        {
            foreach (var go in _actives)
            {
                go.SetActive(value);
            }
        }
    }
}