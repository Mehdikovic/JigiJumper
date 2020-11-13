using JigiJumper.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace JigiJumper.UI
{
    public class RestartWindowUI : MonoBehaviour
    {
        [SerializeField] private GameObject _container = null;
        [SerializeField] private GoogleRewardAdManager _adManager = null;

        [Header("Buttons")]
        [SerializeField] private Button _btnShowAd = null;
        [SerializeField] private Button _btnHome = null;
        [SerializeField] private Button _btnRestart = null;

        private void Awake()
        {
            _adManager.UserWatchedTheAd += () =>
            {
                GameManager.instance.RequestToRestart(RestartMode.Reallocate);
                _container.SetActive(false);
                //todo -> add reward to jumper
            };

            InitialUIComponents();

            _container.SetActive(false);
            GameManager.instance.OnCompleteRestartRequest += OnCompleteRestartRequest;
        }

        private void OnCompleteRestartRequest()
        {
            _container.SetActive(true);
        }

        private void InitialUIComponents()
        {
            _btnRestart.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });

            _btnShowAd.onClick.AddListener(() =>
            {
                _adManager.UserChoseToWatchAd();
            });

            _btnHome.onClick.AddListener(() =>
            {
                // todo -> go to home scene
            });
        }
    }
}