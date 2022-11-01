using UnityEngine;
using UnityEngine.UI;

using JigiJumper.Component;


namespace JigiJumper.Ui {
    public class SelfDestructorUI : MonoBehaviour {
        [SerializeField] private SelfDestructor _destructor = null;
        [SerializeField] private Text _textUI = null;

        private void Awake() {
            _destructor.OnActivateTimer += () => {
                _textUI.text = string.Empty;
                _destructor.OnTimerTick += OnTimerTick;
            };

            _destructor.OnDeactivateTimer += () => {
                _destructor.OnTimerTick -= OnTimerTick;
                _textUI.text = string.Empty;
            };
        }

        private void OnEnable() {
            _textUI.text = string.Empty;
        }

        private void OnDisable() {
            _textUI.text = string.Empty;
        }

        private void OnTimerTick(int timer) {
            _textUI.text = string.Format("{0:0}", timer);
        }
    }
}