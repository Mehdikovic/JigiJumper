using DG.Tweening;
using JigiJumper.Actors;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


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
        ColorAdjustments _colorAdjustment;
        VolumeParameter<float> _parameter;
        Tweener _fadeGray = null;
        Tweener _fadeColorful = null;

        void Awake()
        {
            _wait = new WaitForSeconds(_timer);
            StartCoroutine(ChangeColor());

            if (!_volume.TryGet(out ColorAdjustments colorAdjustment)) { return; }
            _colorAdjustment = colorAdjustment;
            _parameter = new VolumeParameter<float>();

            OnJumperRestart(_jumper.remainingLife);
            _jumper.OnRestart += OnJumperRestart;
        }

        void OnJumperRestart(int remainingLife)
        {
            if (remainingLife == 1)
            {
                if (_fadeColorful != null && _fadeColorful.IsActive()) { _fadeColorful.Kill(); }
                
                _fadeGray = DOTween.To(
                    (value) =>
                    {
                        _parameter.value = value;
                        _colorAdjustment.saturation.SetValue(_parameter);
                    },
                    _parameter.value,
                    -100,
                    1.5f
                );
            }
            else
            {
                if (_fadeGray != null && _fadeGray.IsActive()) { _fadeGray.Kill(); }
                _fadeColorful = DOTween.To(
                    (value) =>
                    {
                        _parameter.value = value;
                        _colorAdjustment.saturation.SetValue(_parameter);
                    },
                    _parameter.value,
                    100,
                    1.5f
                );
            }
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
    }
}