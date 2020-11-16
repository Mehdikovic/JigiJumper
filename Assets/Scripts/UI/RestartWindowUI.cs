using DG.Tweening;
using JigiJumper.Ads;
using JigiJumper.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace JigiJumper.UI
{
    public class RestartWindowUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _container = null;
        
        [Header("Reward-Ad")]
        [SerializeField] private RewardedAd _ads = null;

        [Header("Buttons")]
        [SerializeField] private Button _btnShowAd = null;
        [SerializeField] private Button _btnHome = null;
        [SerializeField] private Button _btnRestart = null;

        private void Awake()
        {
            _ads.OnUnityAdsFinish += (result) =>
            {
                if (result == UnityEngine.Advertisements.ShowResult.Finished)
                {
                    // todo -> add random life added currently is 3
                    GameManager.instance.RequestToRestart(RestartMode.Reallocate, 3);
                    _container.gameObject.SetActive(false);
                }
            };

            InitialUIComponents();


            _container.gameObject.SetActive(false);
            GameManager.instance.OnCompleteRestartRequest += OnCompleteRestartRequest;
        }

        private void OnCompleteRestartRequest()
        {
            _container.gameObject.SetActive(true);
            _container.localScale = new Vector3(0, 0, 1);
            
            _container
                .DOScaleX(1.3f, .3f)
                .onComplete = () => _container.DOScaleX(1f, .1f);

            _container
                .DOScaleY(1.3f, .3f)
                .onComplete = () => _container.DOScaleY(1f, .1f);
        }

        private void InitialUIComponents()
        {
            _btnRestart.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            });

            _btnShowAd.onClick.AddListener(() =>
            {
                _ads.ShowRewardedVideo();
            });

            _btnHome.onClick.AddListener(() =>
            {
                // todo -> go to home scene
            });
        }
    }
}