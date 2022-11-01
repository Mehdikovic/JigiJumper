using JigiJumper.Managers;
using UnityEngine;
using UnityEngine.UI;


namespace JigiJumper.Ui {
    public class HomeWindowUI : WindowUI {
        [Header("Windows")]
        [SerializeField] private WindowUI _gameWindow = null;
        [SerializeField] private WindowUI _recordsWindow = null;
        [SerializeField] private WindowUI _settingsWindow = null;
        [SerializeField] private WindowUI _creditsWindow = null;

        [Header("Home Window Buttons")]
        [SerializeField] private Button _btnStart = null;
        [SerializeField] private Button _btnRecords = null;
        [SerializeField] private Button _btnSettings = null;
        [SerializeField] private Button _btnCredits = null;
        [SerializeField] private Button _btnQuit = null;

        protected override void OnAwake() {
            InitialComponent();

            SetBehaviorActivation(false);
            _selfRectWindow.gameObject.SetActive(false);

            GameManager.instance.jumper.OnPlanetReached += (Actors.PlanetController a, Actors.PlanetController b) => {
                UI.DoShowWindow(_selfRectWindow,
                    onComplete: () => SetBehaviorActivation(true)
                );
            };
        }

        protected override Behaviour[] Behaviors() {
            return new Behaviour[]
            {
                _btnStart,
                _btnRecords,
                _btnSettings,
                _btnCredits,
                _btnQuit,
            };
        }

        private void InitialComponent() {

            _btnStart.onClick.AddListener(() => UI.TransitionTo(this, _gameWindow));
            _btnRecords.onClick.AddListener(() => UI.TransitionTo(this, _recordsWindow));
            _btnSettings.onClick.AddListener(() => UI.TransitionTo(this, _settingsWindow));
            _btnCredits.onClick.AddListener(() => UI.TransitionTo(this, _creditsWindow));
            _btnQuit.onClick.AddListener(() => Application.Quit());
        }
    }
}