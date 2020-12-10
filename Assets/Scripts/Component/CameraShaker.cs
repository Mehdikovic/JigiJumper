using Cinemachine;
using JigiJumper.Actors;
using System.Collections;
using UnityEngine;


namespace JigiJumper.Component {
    public class CameraShaker : MonoBehaviour {
        [SerializeField] private CinemachineVirtualCamera _camera = null;

        private CinemachineBasicMultiChannelPerlin _noise;

        private void Awake() {
            _noise = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            GetComponent<JumperController>().OnRestart += (remianingLife) => {
                StartCoroutine(ShakeTheCamera());
            };
        }

        IEnumerator ShakeTheCamera() {
            _noise.m_AmplitudeGain = 8;
            yield return new WaitForSeconds(.2f);
            _noise.m_AmplitudeGain = 0;
        }
    }
}