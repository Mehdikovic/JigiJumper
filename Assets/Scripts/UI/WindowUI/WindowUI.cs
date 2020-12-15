using JigiJumper.Data;
using System.Collections.Generic;
using UnityEngine;

namespace JigiJumper.Ui {
    public abstract class WindowUi : MonoBehaviour {

        [SerializeField] protected RectTransform _selfRectWindow = null;
        [Header("Settings")]
        [SerializeField] protected SettingData _setting = null;

        Behaviour[] _uiBehaviors;

        protected void Awake() {
            _uiBehaviors = Behaviors();
            OnAwake();
        }

        public IEnumerable<Behaviour> GetUiBehaviors() => _uiBehaviors;

        public RectTransform GetRectWindow() => _selfRectWindow;
        
        protected abstract Behaviour[] Behaviors();

        protected virtual void OnAwake() { }


        public virtual void BeginToHide() { }

        public virtual void EndOfHide() { }

        public virtual void BeginToShow() { }

        public virtual void EndOfShow() { }


        protected void SetBehaviorActivation(bool active) {
            Ui.SetBehaviorActivation(active, _uiBehaviors);
        }
    }
}