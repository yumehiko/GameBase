using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using DG.Tweening;



namespace yumehiko.Audio.Music
{
    /// <summary>
    /// BPMに合わせてイベントを発行する。
    /// </summary>
    public class Metronom : IDisposable
    {
        /// <summary>
        /// 現在のビート数。
        /// </summary>
        public ReadOnlyReactiveProperty<int> BeatCount => beatCount.ToReadOnlyReactiveProperty();

        private IntReactiveProperty beatCount = new IntReactiveProperty();
        private CompositeDisposable managerObserver;
        private Tween beatTween;

        private float beatLength;
        private float startTime;


        public Metronom()
        {
            managerObserver = new CompositeDisposable();

            _ = MusicManager.OnStartMusic
                .Where(clip => clip != null)
                .Subscribe(clip => StartBeat(clip))
                .AddTo(managerObserver);

            _ = MusicManager.OnStartMusic
                .Where(clip => clip == null)
                .Subscribe(_ => StopBeat())
                .AddTo(managerObserver);

            _ = MusicManager.OnStopMusic
                .Subscribe(_ => StopBeat())
                .AddTo(managerObserver);

            _ = MusicManager.IsPausing
                .Where(isPausing => isPausing)
                .Subscribe(_ => PauseBeat())
                .AddTo(managerObserver);

            _ = MusicManager.IsPausing
                .Where(isPausing => !isPausing)
                .Subscribe(_ => ResumeBeat())
                .AddTo(managerObserver);
        }

        public void Dispose()
        {
            beatTween?.Kill();
            managerObserver.Dispose();
        }

        private void StartBeat(MusicClip clip)
        {
            StopBeat();
            startTime = Time.time;

            if (clip.BPM <= 0.0f)
            {
                return;
            }
            beatLength = 60.0f / clip.BPM;
            beatTween = DOVirtual.DelayedCall(beatLength, () => Beat(), true);
        }

        private void Beat()
        {
            beatCount.Value++;
            float currentTime = Time.time;
            float correctBeatTime = beatLength * beatCount.Value + startTime;
            float nextBeatLength = correctBeatTime - currentTime + beatLength;
            beatTween = DOVirtual.DelayedCall(nextBeatLength, () => Beat(), true);
        }

        private void StopBeat()
        {
            beatTween?.Kill();
            beatCount.Value = 0;
        }

        private void PauseBeat()
        {
            beatTween?.Pause();
        }

        private void ResumeBeat()
        {
            beatTween?.Play();
        }
    }
}