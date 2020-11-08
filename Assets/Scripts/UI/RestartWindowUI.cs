using JigiJumper.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JigiJumper.UI
{

    public class RestartWindowUI : MonoBehaviour
    {
        [SerializeField] private GameObject _container = null;

        [Header("Buttons")]
        [SerializeField] private Button _btnShowAd = null;
        [SerializeField] private Button _btnHome = null;
        [SerializeField] private Button _btnRestart = null;

        private void Awake()
        {
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

            _btnShowAd.onClick.AddListener(() => {
                //show ad
                GameManager.instance.RequestToRestart(RestartMode.Reallocate);
                _container.SetActive(false);
            });

            _btnHome.onClick.AddListener(() => {
                // todo -> go to home scene
            });
        }
    }
}