using JigiJumper.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace JigiJumper.Ui {
    public class HomeWindowUi : WindowUi {
        [Header("Windows")]
        [SerializeField] private WindowUi _gameWindow = null;
        [SerializeField] private WindowUi _recordsWindow = null;
        [SerializeField] private WindowUi _settingsWindow = null;

        [Header("Home Window Buttons")]
        [SerializeField] private Button _btnStart = null;
        [SerializeField] private Button _btnRecords = null;
        [SerializeField] private Button _btnSettings = null;
        [SerializeField] private Button _btnQuit = null;

        protected override void OnAwake() {
            InitialComponent();

            SetBehaviorActivation(false);
            _selfRectWindow.gameObject.SetActive(false);

            GameManager.instance.jumper.OnPlanetReached += (Actors.PlanetController a, Actors.PlanetController b) => {
                Ui.DoShowWindow(_selfRectWindow,
                    onComplete: () => SetBehaviorActivation(true)
                );
            };
        }

        protected override Behaviour[] Behaviors() {
            return  new Behaviour[]
            {
                _btnStart,
                _btnRecords,
                _btnSettings,
                _btnQuit,
            };
        }

        private void InitialComponent() {
            
            _btnStart.onClick.AddListener(OnBtnStartClicked);
            _btnRecords.onClick.AddListener(OnBtnRecordClicked);
            _btnSettings.onClick.AddListener(OnBtnSettingsClicked);
            _btnQuit.onClick.AddListener(OnBtnQuitClicked);
        }

        private void OnBtnStartClicked() {
            Ui.TransitionTo(this, _gameWindow);
        }

        private void OnBtnRecordClicked() {
            Ui.TransitionTo(this, _recordsWindow);
        }

        private void OnBtnSettingsClicked() {
            Ui.TransitionTo(this, _settingsWindow);
        }

        private void OnBtnQuitClicked() {
            Application.Quit();
        }
    }
}