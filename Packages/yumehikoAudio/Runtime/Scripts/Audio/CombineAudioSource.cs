using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;

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

        public bool IsPlaying => mainSource.isPlaying;

        private static Sequence crossFadeSequence;
        private CancellationToken dToken;

        private void Awake()
        {
            if (transform.parent == null)
            {
                DontDestroyOnLoad(this);
            }

            if (MusicManager.IsInstalled)
            {
                return;
            }

            dToken = this.GetCancellationTokenOnDestroy();
            MusicManager.InstallSources(this);
        }

        /// <summary>
        /// オーディオソースを破棄する。
        /// </summary>
        public void UnInstall()
        {
            Destroy(this);
        }

        public void SwitchPause(bool isPause)
        {
            float value = isPause ? 0.0f : 1.0f;
            mainSource.pitch = value;
            fadeSource.pitch = value;
        }

        public async UniTaskVoid StopMusic(float fadeOutDuration = 1.0f)
        {
            crossFadeSequence?.Complete(true);
            await mainSource.DOFade(0.0f, fadeOutDuration).ToUniTask(cancellationToken: dToken);
        }

        /// <summary>
        /// BGM即時切り替え。
        /// </summary>
        /// <param name="musicClip"></param>
        public void InstantSwap(MusicClip musicClip)
        {
            crossFadeSequence?.Complete(true);

            mainSource.clip = musicClip.Clip;
            mainSource.volume = 1.0f;
            mainSource.Play();

            fadeSource.volume = 0.0f;
            fadeSource.Stop();
        }

        public void SetLoopMode(bool doLoop)
        {
            mainSource.loop = doLoop;
            fadeSource.loop = doLoop;
        }

        /// <summary>
        /// ふたつのBGMをクロスフェードで切り替える。
        /// </summary>
        /// <param name="duration"></param>
        public async UniTaskVoid CrossFadeSwap(MusicClip musicClip, float duration)
        {
            crossFadeSequence?.Complete(true);

            fadeSource.clip = musicClip.Clip;
            fadeSource.volume = 0.0f;
            fadeSource.Play();

            crossFadeSequence = DOTween.Sequence()
                .Append(fadeSource.DOFade(1.0f, duration))
                .Join(mainSource.DOFade(0.0f, duration))
                .OnComplete(() => SwapSource());

            await crossFadeSequence.Play().ToUniTask(cancellationToken: dToken);
        }

        /// <summary>
        /// BGM用のソースを主客転倒する。
        /// </summary>
        private void SwapSource()
        {
            AudioSource tempSource = fadeSource;
            fadeSource = mainSource;
            mainSource = tempSource;
            fadeSource.Stop();
        }
    }
}