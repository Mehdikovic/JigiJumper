using System;
using UnityEngine;

#pragma warning disable 0649

namespace JigiJumper.Data {

    [System.Serializable]
    public struct BgMusicInfo {
        public AudioClip clip;
        public float start;
        [Range(0f, 1f)]
        [SerializeField] float endPercent;

        public float GetWaitTime() {
            if (clip.length <= start) { return 0f; }
            return (clip.length - start) * endPercent;
        }
    }

    [CreateAssetMenu(fileName = "Bg Music Data", menuName = "Data/BgMusicData")]
    public class BgMusicData : ScriptableObject {
        [SerializeField] BgMusicInfo[] _clips = null;

        int _index = -1;

        private void OnEnable() {
            _index = -1;
        }

        public BgMusicInfo Next() {
            ++_index;
            _index %= _clips.Length;
            return _clips[_index];
        }

        public bool HasMusic() {
            return (_clips != null && _clips.Length > 0);
        }

        public void OnBeforeSerialize() {
            _index = -1;
        }

        public void OnAfterDeserialize() {
            //throw new NotImplementedException();
        }
    }
}