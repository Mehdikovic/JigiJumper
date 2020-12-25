using System;
using UnityEngine;


namespace JigiJumper.Data {
    [System.Serializable]
    public enum LevelType {
        Easy = 0,
        Normal = 10,
        Hard = 20,
    }

#pragma warning disable 0414

    [CreateAssetMenu(fileName = "Settings", menuName = "Data/Settings")]
    public class SettingData : ScriptableObject {
        private const string MUSIC_VOL = "musicVol";
        private const string IN_GAME_SOUND = "inGameSoundVol";
        private const string BANNER_OPTION = "bannerShowOption";

        [Header("Ads Conf")]
        [SerializeField] private bool _enable = true;
        [SerializeField] private string _gameId_ANDROID = "3872205";
        [SerializeField] private string _gameId_IOS = "3872204";
        [SerializeField] private bool _testMode = true;

        [Header("Level Type")]
        public LevelType levelType = LevelType.Easy;

        public string gameId {
            get {
#if UNITY_ANDROID
                return _gameId_ANDROID;
#elif UNITY_IOS
                return _gameId_IOS;
#else
                return "0";
#endif
            }
        }
        public bool testMode => _testMode;
        public bool adEnable => _enable;
        public void SetMusicVol(float musicVol) {
            musicVol = Mathf.Clamp(musicVol, -80, 0);
            PlayerPrefs.SetFloat(MUSIC_VOL, musicVol);
        }

        public float GetMusicVol() {
            if (!PlayerPrefs.HasKey(MUSIC_VOL)) { return 0; }

            return PlayerPrefs.GetFloat(MUSIC_VOL);
        }

        public void SetInGameSound(float inGameSoundVol) {
            inGameSoundVol = Mathf.Clamp(inGameSoundVol, -80, 0);
            PlayerPrefs.SetFloat(IN_GAME_SOUND, inGameSoundVol);
        }

        public float GetInGameSound() {
            if (!PlayerPrefs.HasKey(IN_GAME_SOUND)) { return 0; }

            return PlayerPrefs.GetFloat(IN_GAME_SOUND);
        }

        public void SetShowBannerOption(bool value) {
            PlayerPrefs.SetInt(BANNER_OPTION, Convert.ToInt32(value));
        }

        public bool GetShowBannerOption() {
            if (!PlayerPrefs.HasKey(BANNER_OPTION)) { return true; }
            return Convert.ToBoolean(PlayerPrefs.GetInt(BANNER_OPTION));
        }
    }
}