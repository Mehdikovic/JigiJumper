using DG.Tweening;
using JigiJumper.Actors;
using JigiJumper.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using JigiJumper.Utils;
using System;

namespace JigiJumper.Component
{

    public class CameraVFX : MonoBehaviour
    {
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

        void Awake()
        {
            _wait = new WaitForSeconds(_timer);
            StartCoroutine(ChangeColor());

            SettingTheColorAdjusment();
            SettingVignette();
        }

        IEnumerator ChangeColor()
        {
            int H = 165;
            bool increase = true;

            while (true)
            {
                H = increase ? ++H : --H;

                _camera.backgroundColor = Color.HSVToRGB(
                    H / HSV_H_MAX,
                    HSV_s / HSV_S_MAX,
                    HSV_v / HSV_V_MAX
                );

                yield return _wait;

                if (H == 320)
                {
                    increase = false;
                    --H;
                }
                else if (H == 165)
                {
                    increase = true;
                    ++H;
                }
            }
        }

        private void SettingTheColorAdjusment()
        {
            if (!_volume.TryGet(out ColorAdjustments colorAdjustment)) { return; }

            Tweener fadeGray = null;
            Tweener fadeColorful = null;
            VolumeParameter<float> colorAdjustParam = new VolumeParameter<float>();

            Action<int> onRestartDel = (remainingLife) =>
            {
                if (remainingLife == 1)
                {
                    if (fadeColorful != null && fadeColorful.IsActive()) { fadeColorful.Kill(); }

                    fadeGray = DOTween.To(
                        (value) =>
                        {
                            colorAdjustParam.value = value;
                            colorAdjustment.saturation.SetValue(colorAdjustParam);
                        },
                        colorAdjustParam.value,
                        -100,
                        1.5f
                    );
                }
                else
                {
                    if (fadeGray != null && fadeGray.IsActive()) { fadeGray.Kill(); }
                    fadeColorful = DOTween.To(
                        (value) =>
                        {
                            colorAdjustParam.value = value;
                            colorAdjustment.saturation.SetValue(colorAdjustParam);
                        },
                        colorAdjustParam.value,
                        100,
                        1.5f
                    );
                }
            };

            onRestartDel(_jumper.remainingLife);
            _jumper.OnRestart += onRestartDel;
        }
        private void SettingVignette()
        {
            var gManager = GameManager.instance;
            if (gManager.levelType != Data.LevelType.Hard) { return; }
            if (!_volume.TryGet(out Vignette vignette)) { return; }

            VolumeParameter<float> vignetteParam = new VolumeParameter<float>();

            var seq = DOTween.Sequence().SetAutoKill(false);
            seq.SetLoops(-1);

            Action<int> onLevelChangedDel = (newLevel) =>
            {
                //todo -> vignette
                //float remap = Utility.Map(newLevel, 1, 5, 0.14f, 1f);
                //var seq1 = DOTween.To(
                //    (value) =>
                //    {
                //        vignetteParam.value = value;
                //        vignette.intensity.SetValue(vignetteParam);
                //    },
                //    vignetteParam.value,
                //    remap,
                //    1f
                //);
                //var seq2 = DOTween.To(
                //     (value) =>
                //     {
                //         vignetteParam.value = value;
                //         vignette.intensity.SetValue(vignetteParam);
                //     },
                //     vignetteParam.value,
                //     0.14f,
                //     1f
                // );
            };

            onLevelChangedDel(gManager.currentLevel);
            gManager.OnLevelChanged += onLevelChangedDel;
        }
    }
}