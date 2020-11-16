using DG.Tweening;
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
            DoTweenAnimation(_txtLevel.rectTransform);
        }

        private void OnJumpeRestart(int remainingLife)
        {
            _txtLife.text = remainingLife.ToString();
            //DoTweenAnimation(_txtLife.rectTransform);
            DoTweenShake();
        }

        private void DoTweenAnimation(RectTransform rectTramsfrom)
        {
            rectTramsfrom
                .DOScaleX(2f, 0.2f)
                .onComplete = () => _txtLevel.rectTransform.DOScaleX(5f, 0.2f)
                .onComplete = () => _txtLevel.rectTransform.DOScaleX(3f, 0.1f);

            rectTramsfrom
                .DOScaleY(2f, 0.2f)
                .onComplete = () => _txtLevel.rectTransform.DOScaleY(5f, 0.2f)
                .onComplete = () => _txtLevel.rectTransform.DOScaleY(3f, 0.1f);
        }

        private void DoTweenShake()
        {
            _txtLife.rectTransform.DOShakeAnchorPos(1f);
        }
    }
}