using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using JigiJumper.Component;


namespace JigiJumper.Ui {
    public class SelfDestructorUi : MonoBehaviour {
        [SerializeField] private SelfDestructor _destructor = null;
        [SerializeField] private Text _textUI = null;

        private Coroutine _routine;

        private void Awake() {
            _textUI.text = string.Empty;
        }

        private void OnEnable() {
            StopCoroutine();
            _routine = StartCoroutine(UpdateTimerUI());
        }

        private void OnDisable() {
            StopCoroutine();
        }

        IEnumerator UpdateTimerUI() {
            while (true) {
                if (_destructor.isActiveComponent) {
                    _textUI.text = string.Format("{0:0}", _destructor.timer);
                } else {
                    _textUI.text = string.Empty;
                }
                yield return null;
            }
        }

        void StopCoroutine() {
            if (_routine == null) { return; }

            StopCoroutine(_routine);
            _routine = null;
        }
    }
}