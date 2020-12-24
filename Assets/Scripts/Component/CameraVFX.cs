using DG.Tweening;
using JigiJumper.Actors;
using JigiJumper.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using JigiJumper.Utils;


namespace JigiJumper.Component {
    public class CameraVFX : MonoBehaviour {
        private const float HSV_S_MAX = 100f;
        private const float HSV_V_MAX = 100f;
        private const float HSV_H_MAX = 360f;

        [SerializeField] private Camera _camera = null;

        [SerializeField] private JumperController _jumper = null;

        [Header("POST PROCESSING PROFILE")]
        [SerializeField] private VolumeProfile _volume = null;


        [Header("Color HSV")]
        [SerializeField] private int HSV_s = 43;
        [SerializeField] private int HSV_v = 30;

        [Header("Time Between Changes")]
        [SerializeField] private float _timer = 0.05f;

        WaitForSeconds _wait;

        Coroutine _vignetteCoroutine;
        int _prevLevel = 1;

        void Awake() {
            _wait = new WaitForSeconds(_timer);
            StartCoroutine(ChangeColor());

            SettingTheColorAdjusment();
            SettingVignette();
        }

        IEnumerator ChangeColor() {
            int H = 165;
            bool increase = true;

            while (true) {
                H = increase ? ++H : --H;

                _camera.backgroundColor = Color.HSVToRGB(
                    H / HSV_H_MAX,
                    HSV_s / HSV_S_MAX,
                    HSV_v / HSV_V_MAX
                );

                yield return _wait;

                if (H == 320) {
                    increase = false;
                    --H;
                } else if (H == 165) {
                    increase = true;
                    ++H;
                }
            }
        }

        private void SettingTheColorAdjusment() {
            if (!_volume.TryGet(out ColorAdjustments colorAdjustment)) { return; }

            Tweener fadeGray = null;
            Tweener fadeColorful = null;
            VolumeParameter<float> colorAdjustParam = new VolumeParameter<float>();

            Action<int, RestartMode> onRestartDel = (remainingLife, _) => {
                if (remainingLife == 1) {
                    if (fadeColorful != null && fadeColorful.IsActive()) { fadeColorful.Kill(); }

                    fadeGray = DOTween.To(
                        (value) => {
                            colorAdjustParam.value = value;
                            colorAdjustment.saturation.SetValue(colorAdjustParam);
                        },
                        colorAdjustParam.value,
                        -100,
                        1.5f
                    );
                } else {
                    if (fadeGray != null && fadeGray.IsActive()) { fadeGray.Kill(); }
                    fadeColorful = DOTween.To(
                        (value) => {
                            colorAdjustParam.value = value;
                            colorAdjustment.saturation.SetValue(colorAdjustParam);
                        },
                        colorAdjustParam.value,
                        100,
                        1.5f
                    );
                }
            };

            onRestartDel(_jumper.remainingLife, RestartMode.Reallocate);
            _jumper.OnRestart += onRestartDel;
        }

        private void SettingVignette() {
            var gm = GameManager.instance;
            if (gm.levelType == Data.LevelType.Easy) { return; }
            
            if (!_volume.TryGet(out Vignette vignette)) { return; }
            
            Action<int> onLevelChanged = (newLevel) => {
                int lvlCount = 1;
                float waitTime = 1f;
                float newVignetteValue = 0.14f;
                switch (gm.levelType) {
                    /* case Data.LevelType.Easy:
                         newVignetteValue = Utility.Map(newLevel, 1, 100, 0.14f, 1f);
                         lvlCount = 1;
                         waitTime = 1f;
                         break; */
                    case Data.LevelType.Normal:
                        newVignetteValue = Utility.Map(newLevel, 1, 50, 0.14f, 1f);
                        lvlCount = UnityEngine.Random.Range(1, 3);
                        waitTime = UnityEngine.Random.Range(.5f, 2f);
                        break;
                    case Data.LevelType.Hard:
                        newVignetteValue = Utility.Map(newLevel, 1, 10, 0.14f, 1f);
                        lvlCount = UnityEngine.Random.Range(1, 5);
                        waitTime = UnityEngine.Random.Range(.3f, 4f);
                        break;
                }

                if (_vignetteCoroutine != null && newLevel - _prevLevel > lvlCount) {
                    _prevLevel = newLevel;
                    ResetVignette(vignette);
                    return;
                }

                if (_vignetteCoroutine != null) { return; }

                if (UnityEngine.Random.Range(0, 2) == 1) {
                    _prevLevel = newLevel;
                    _vignetteCoroutine = StartCoroutine(ChangeVignetteThickness(vignette, newVignetteValue, waitTime));
                }
            };

            gm.OnLevelChanged += onLevelChanged;
        }
        IEnumerator ChangeVignetteThickness(Vignette vignette, float newVignetteValue, float waitTime) {
            var vignetteParam = new VolumeParameter<float>();
            var waitForSecond = new WaitForSeconds(waitTime);

            while (true) {
                DOTween.To(
                    (value) => {
                        vignetteParam.value = value;
                        vignette.intensity.SetValue(vignetteParam);
                    },
                    vignette.intensity.value,
                    newVignetteValue,
                    waitTime
                );
                yield return waitForSecond;
                DOTween.To(
                    (value) => {
                        vignetteParam.value = value;
                        vignette.intensity.SetValue(vignetteParam);
                    },
                    vignette.intensity.value,
                    0.14f,
                    waitTime
                );
                yield return waitForSecond;
            }
        }

        private void ResetVignette(Vignette vignette) {
            if (_vignetteCoroutine != null) {
                StopCoroutine(_vignetteCoroutine);
                _vignetteCoroutine = null;
            }
            var vignetteParam = new VolumeParameter<float>();
            DOTween.To(
                (value) => {
                    vignetteParam.value = value;
                    vignette.intensity.SetValue(vignetteParam);
                },
                vignette.intensity.value,
                0.14f,
                1f
            );
        }

        private void OnDisable() {
            if (!_volume.TryGet(out Vignette vignette)) { return; }
            var vignetteParam = new VolumeParameter<float>();
            vignetteParam.value = 0.14f;
            vignette.intensity.SetValue(vignetteParam);
        }
    }
}