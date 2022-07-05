using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.Audio.Music
{
    /// <summary>
    /// オーディオソースをMusicManagerへ渡す。
    /// </summary>
    public class CombineAudioSource : MonoBehaviour
    {
        /// <summary>
        /// BGMのオーディオソース。
        /// </summary>
        [SerializeField] private AudioSource mainSource = default;

        /// <summary>
        /// BGMのクロスフェード用ソース。
        /// </summary>
        [SerializeField] private AudioSource fadeSource = default;

        public AudioSource MainSource => mainSource;
        public AudioSource FadeSource => fadeSource;

        private void Awake()
        {
            MusicManager.InstallSources(this);
            DontDestroyOnLoad(this);
        }

        /// <summary>
        /// オーディオソースを破棄する。
        /// </summary>
        public void UnInstall()
        {
            Destroy(this);
        }

    }
}