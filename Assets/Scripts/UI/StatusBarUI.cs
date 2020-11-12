using JigiJumper.Managers;
using TMPro;
using UnityEngine;


namespace JigiJumper.UI
{
    public class StatusBarUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _txtLevel = null;
        [SerializeField] private TextMeshProUGUI _txtLife = null;

        GameManager _manager;

        private void Awake()
        {
            _manager = GameManager.instance;
            
            _txtLevel.text = _manager.currentLevel.ToString();
            _txtLife.text = _manager.jumper.remainingLife.ToString();
        }

        private void OnEnable()
        {
            _manager.OnLevelChanged += OnLevelChanged;
            _manager.jumper.OnRestart += OnJumpeRestart;
        }

        private void OnDisable()
        {
            _manager.OnLevelChanged -= OnLevelChanged;
            _manager.jumper.OnRestart -= OnJumpeRestart;
        }

        private void OnLevelChanged(int newLevel)
        {
            _txtLevel.text = newLevel.ToString();
        }

        private void OnJumpeRestart()
        {
            _txtLife.text = _manager.jumper.remainingLife.ToString();
        }


    }
}