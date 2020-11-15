using System.Collections;
using UnityEngine;


namespace JigiJumper.Component
{

    public class CameraVFX : MonoBehaviour
    {
        private const float S = 43 / 100f;
        private const float V = 30 / 100f;
        private const float HRange = 360f;

        [SerializeField] private Camera _camera = null;
        [SerializeField] private float _timer = 0.1f;

        WaitForSeconds _wait;

        void Awake()
        {
            _wait = new WaitForSeconds(_timer);
            StartCoroutine(ChangeColor());
        }

        IEnumerator ChangeColor()
        {
            int H = 165;
            bool increase = true;
            
            while (true)
            {
                H = increase ? ++H : --H;
                _camera.backgroundColor = Color.HSVToRGB(H / HRange, S, V); ;
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