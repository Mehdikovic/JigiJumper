using JigiJumper.Managers;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


namespace JigiJumper.Ui {
    public class SettingsWindowUi : WindowUi {
        private const string MUSIC_VOLUME = "MusicVolume";
        private const string IN_GAME_VOLUME = "InGameVolume";

        [Header("Windows")]
        [SerializeField] private WindowUi _homeWindow = null;

        [Header("AudioMixer")]
        [SerializeField] private AudioMixer _audioMixer = null;

        [Header("UI Behavoirs")]
        [SerializeField] private Slider _musicSlider = null;
        [SerializeField] private Slider _inGameSoundSlider = null;
        [SerializeField] private Toggle _toggle = null;
        [SerializeField] private Button _btnBack = null;
        [SerializeField] private Image _toggleImage = null;

        [Header("Gfxs")]
        [SerializeField] private GameObject _toggleHolder = null;

        protected override void OnAwake() {
            InitialComponent();

            SetBehaviorActivation(false);
            _selfRectWindow.gameObject.SetActive(false);
        }

        protected override Behaviour[] Behaviors() {
            return new Behaviour[] {
                _musicSlider,
                _inGameSoundSlider,
                _toggle,
                _btnBack,
            };
        }

        private void InitialComponent() {
            _musicSlider.onValueChanged.AddListener((value) => {
                _setting.SetMusicVol(value);
                _audioMixer.SetFloat(MUSIC_VOLUME, value);
            });

            _inGameSoundSlider.onValueChanged.AddListener((value) => {
                _setting.SetInGameSound(value);
                _audioMixer.SetFloat(IN_GAME_VOLUME, value);
            });

            _btnBack.onClick.AddListener(() => {
                if (_homeWindow == null) {
                    SetBehaviorActivation(false);
                    Ui.DoHideWindow(_selfRectWindow, () => SetBehaviorActivation(true));
                } else {
                    Ui.TransitionTo(this, _homeWindow);
                }
            });


            _toggle.onValueChanged.AddListener((value) => {
                _setting.SetShowBannerOption(value);
                _toggleImage.enabled = value;
            });

            // Setteing the configs
            _musicSlider.value = _setting.GetMusicVol();
            _inGameSoundSlider.value = _setting.GetInGameSound();
            _toggle.isOn = _setting.GetShowBannerOption();
            _toggleImage.enabled = _toggle.isOn;

            if (!_setting.adEnable && _toggleHolder != null) {
                _toggleHolder.SetActive(false);
            }

        }

    }
}