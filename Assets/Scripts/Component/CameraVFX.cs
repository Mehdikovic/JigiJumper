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

        void Awake()
        {
            _wait = new WaitForSeconds(_timer);
            StartCoroutine(ChangeColor());

            if (!_volume.TryGet(out ColorAdjustments colorAdjustment)) { return; }
            
            VolumeParameter<float> parameter = new VolumeParameter<float>();
            
            _jumper.OnRestart += (remainingLife) =>
            {
                if (remainingLife == 1)
                {
                    parameter.value = -100;
                    colorAdjustment.saturation.SetValue(parameter);
                }
                else
                {
                    parameter.value = 100;
                    colorAdjustment.saturation.SetValue(parameter);
                }
            };
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