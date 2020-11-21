using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


namespace JigiJumper.UI
{
    public class SettingsWindowUI : WindowUI
    {
        private const string MUSIC_VOLUME = "MusicVolume";
        private const string IN_GAME_VOLUME = "InGameVolume";

        [Header("Windows")]
        [SerializeField] private WindowUI _homeWindow = null;

        [Header("AudioMixer")]
        [SerializeField] private AudioMixer _audioMixer = null;

        [SerializeField] private Slider _musicSlider = null;
        [SerializeField] private Slider _inGameSoundSlider = null;
        [SerializeField] private Toggle _toggle = null;
        [SerializeField] private Button _btnBack = null;
        [SerializeField] private Image _toggleImage = null;

        private void Awake()
        {
            InitialComponent();

            _selfRect.gameObject.SetActive(false);
        }

        private void InitialComponent()
        {
            _behaviorUIs = new Behaviour[]
            {
                _musicSlider,
                _inGameSoundSlider,
                _toggle,
                _btnBack,
            };

            _musicSlider.onValueChanged.AddListener((value) =>
            {
                _setting.music = value;
                _audioMixer.SetFloat(MUSIC_VOLUME, value);
                print("called");
            });

            _inGameSoundSlider.onValueChanged.AddListener((value) =>
            {
                _setting.inGameSound = value;
                _audioMixer.SetFloat(IN_GAME_VOLUME, value);
                print("called");
            });

            _btnBack.onClick.AddListener(() => TransitionToWindow(this, _homeWindow));

            _toggle.onValueChanged.AddListener((value) =>
            {
                _setting.showBanner = value;
                _toggleImage.enabled = value;
            });

            // Setteing the configs
            _musicSlider.value = _setting.music;
            _inGameSoundSlider.value = _setting.inGameSound;
            _toggle.isOn = _setting.showBanner;
            _toggleImage.enabled = _toggle.isOn;
        }

    }
}