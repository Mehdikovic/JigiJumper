using JigiJumper.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace JigiJumper.UI
{
    public class HomeWindowUI : WindowUI
    {
        [Header("Windows")]
        [SerializeField] private WindowUI _gameWindow = null;
        [SerializeField] private WindowUI _recordsWindow = null;
        [SerializeField] private WindowUI _settingsWindow = null;

        [Header("Home Window Buttons")]
        [SerializeField] private Button _btnStart = null;
        [SerializeField] private Button _btnRecords = null;
        [SerializeField] private Button _btnSettings = null;
        [SerializeField] private Button _btnQuit = null;


        private void Awake()
        {
            InitialComponent();

            SetActivation(false, _behaviorUIs);
            _selfRect.gameObject.SetActive(false);

            GameManager.instance.jumper.OnPlanetReached += (Actors.PlanetController arg1, Actors.PlanetController arg2) =>
            {
                Utils.DoTweenUtility.DoShowWindow(_selfRect,
                    onComplete: () => SetActivation(true, _behaviorUIs)
                );
            };
        }

        private void InitialComponent()
        {
            _behaviorUIs = new Behaviour[]
            {
                _btnStart,
                _btnRecords,
                _btnSettings,
                _btnQuit,
            };

            _btnStart.onClick.AddListener(OnBtnStartClicked);
            _btnRecords.onClick.AddListener(OnBtnRecordClicked);
            _btnSettings.onClick.AddListener(OnBtnSettingsClicked);
            _btnQuit.onClick.AddListener(OnBtnQuitClicked);
        }

        private void OnBtnStartClicked()
        {
            TransitionToWindow(this, _gameWindow);
        }

        private void OnBtnRecordClicked()
        {
            TransitionToWindow(this, _recordsWindow);
        }

        private void OnBtnSettingsClicked()
        {
            TransitionToWindow(this, _settingsWindow);
        }

        private void OnBtnQuitClicked()
        {
            Application.Quit();
        }

    }
}