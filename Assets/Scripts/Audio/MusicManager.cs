using UnityEngine;
using DG.Tweening;
using UniRx;
using yumehiko.Resident;

namespace yumehiko.Audio.Music
{

    public static class MusicManager
    {
        /// <summary>
        /// メトロノーム。
        /// </summary>
        public static Metronom Metronom { get; }

        /// <summary>
        /// 現在再生中のクリップ。
        /// </summary>
        public static MusicClip CurrentClip { get; private set; }

        /// <summary>
        /// 再生が始まったとき。
        /// </summary>
        public static System.IObservable<MusicClip> OnStartMusic => onStartMusic;

        /// <summary>
        /// 再生を停止したとき。
        /// </summary>
        public static System.IObservable<Unit> OnStopMusic => onStopMusic;

        /// <summary>
        /// 現在再生中のクリップの、再生位置（正規）。
        /// </summary>
        public static ReadOnlyReactiveProperty<float> PlayTime => playTime.ToReadOnlyReactiveProperty();

        /// <summary>
        /// 現在、一時停止中か。
        /// </summary>
        public static ReadOnlyReactiveProperty<bool> IsPausing => isPausing.ToReadOnlyReactiveProperty();

        /// <summary>
        /// 一時停止時、音楽も停止するか
        /// </summary>
        public static bool DependPause { get; private set; } = false;

        private static AudioSource mainSource;
        private static AudioSource fadeSource;
        private static CombineAudioSource combineAudioSource;
        private static Sequence crossFadeSequence;

        private static Subject<MusicClip> onStartMusic = new Subject<MusicClip>();
        private static Subject<Unit> onStopMusic = new Subject<Unit>();
        private static BoolReactiveProperty isPausing = new BoolReactiveProperty();
        private static CompositeDisposable pauseModeDisposables;

        private static Tween clipLengthTween;
        private static FloatReactiveProperty playTime = new FloatReactiveProperty(0.0f);



        static MusicManager()
        {
            Metronom = new Metronom();
        }

        /// <summary>
        /// オーディオソースを取得する。
        /// </summary>
        /// <param name="combineAudioSource"></param>
        public static void InstallSources(CombineAudioSource combineAudioSource)
        {
            MusicManager.combineAudioSource?.UnInstall();

            MusicManager.combineAudioSource = combineAudioSource;
            mainSource = combineAudioSource.MainSource;
            fadeSource = combineAudioSource.FadeSource;
        }

        /// <summary>
        /// 指定したClipを再生する。
        /// </summary>
        /// <param name="musicClip">再生するクリップ</param>
        /// <param name="duration">フェードにかかる時間</param>
        /// <param name="forceHead">必ず曲の初めから再生するか。</param>
        public static void PlayMusic(MusicClip musicClip, float duration = 1.0f, bool forceHead = false)
        {
            //オーディオソース未構築なら、無視。
            if (combineAudioSource == null)
            {
                Debug.LogWarning("オーディオソース未定義");
                return;
            }

            //同じClipが指定されたなら、なにもしない。
            if (CurrentClip == musicClip && !forceHead)
            {
                return;
            }

            //clipがnullなら、単に音楽を停止。
            if (musicClip == null)
            {
                StopMusic(duration);
                return;
            }

            //BGMが再生中でないなら、フェードなし即時再生。
            if (!mainSource.isPlaying)
            {
                InstantSwap(musicClip);
                return;
            }

            //指定時間かけて、クロスフェードする。
            CrossFadeSwap(musicClip, duration);
        }

        /// <summary>
        /// 再生中のBGMを指定時間かけてフェードアウトで停止する。
        /// </summary>
        /// <param name="fadeOutDuration">フェードアウトにかかる時間。0なら即時。</param>
        public static void StopMusic(float fadeOutDuration = 1.0f)
        {
            //オーディオソース未構築なら、無視。
            if (combineAudioSource == null)
            {
                return;
            }

            onStopMusic.OnNext(Unit.Default);

            clipLengthTween?.Kill();
            crossFadeSequence?.Kill(true);

            //指定時間かけてフェードアウトして停止。
            crossFadeSequence = DOTween.Sequence()
                .Append(mainSource.DOFade(0.0f, fadeOutDuration))
                .OnComplete(() =>
                {
                    mainSource.Stop();
                    CurrentClip = null;
                    playTime.Value = 0.0f;
                });

        }

        /// <summary>
        /// ループするかどうかを切り替える。
        /// </summary>
        /// <param name="doLoop"></param>
        public static void SetLoopMode(bool doLoop)
        {
            //オーディオソース未構築なら、無視。
            if (combineAudioSource == null)
            {
                return;
            }

            mainSource.loop = doLoop;
            fadeSource.loop = doLoop;
        }

        /// <summary>
        /// 一時停止時、音楽も停止するかをセット。
        /// </summary>
        /// <param name="dependPause">trueなら、一時停止時、音楽を停止する。</param>
        public static void SetPauseMode(bool dependPause)
        {
            if (DependPause == dependPause)
            {
                return;
            }

            DependPause = dependPause;

            if (dependPause)
            {
                pauseModeDisposables = new CompositeDisposable();
                _ = PauseManager.IsPause.Where(isPause => isPause).Subscribe(_ => PauseMusic()).AddTo(pauseModeDisposables);
                _ = PauseManager.IsPause.Where(isPause => !isPause).Subscribe(_ => ResumeMusic()).AddTo(pauseModeDisposables);
            }
            else
            {
                ResumeMusic();
                pauseModeDisposables?.Dispose();
            }
        }



        /// <summary>
        /// 一時停止。
        /// </summary>
        private static void PauseMusic()
        {
            mainSource.pitch = 0.0f;
            fadeSource.pitch = 0.0f;
            isPausing.Value = true;
        }

        /// <summary>
        /// 一時停止を解除。
        /// </summary>
        private static void ResumeMusic()
        {
            mainSource.pitch = 1.0f;
            fadeSource.pitch = 1.0f;
            isPausing.Value = false;
        }

        /// <summary>
        /// ふたつのBGMをクロスフェードで切り替える。
        /// </summary>
        /// <param name="duration"></param>
        private static void CrossFadeSwap(MusicClip musicClip, float duration)
        {
            ChangeClip(fadeSource, musicClip);
            onStartMusic.OnNext(musicClip);
            fadeSource.Play();

            crossFadeSequence?.Complete();

            crossFadeSequence = DOTween.Sequence()
                .OnStart(() => fadeSource.volume = 0.0f)
                .Append(fadeSource.DOFade(1.0f, duration))
                .Join(mainSource.DOFade(0.0f, duration))
                .OnComplete(() => SwapSource());
        }

        /// <summary>
        /// BGM即時切り替え。
        /// </summary>
        /// <param name="musicClip"></param>
        private static void InstantSwap(MusicClip musicClip)
        {
            crossFadeSequence.Kill(true);

            ChangeClip(mainSource, musicClip);
            onStartMusic.OnNext(musicClip);
            mainSource.volume = 1.0f;
            mainSource.Play();

            fadeSource.volume = 0.0f;
            fadeSource.Stop();
        }

        /// <summary>
        /// BGM用のソースを主客転倒する。
        /// </summary>
        private static void SwapSource()
        {
            AudioSource tempSource = fadeSource;
            fadeSource = mainSource;
            mainSource = tempSource;
            fadeSource.Stop();
        }

        /// <summary>
        /// クリップを入れ替えて、そのクリップ再生時間を測り始める。
        /// </summary>
        /// <param name="musicClip"></param>
        private static void ChangeClip(AudioSource targetSource, MusicClip musicClip)
        {
            clipLengthTween?.Kill();

            targetSource.clip = musicClip.Clip;
            CurrentClip = musicClip;
            float length = musicClip.Length;
            playTime.Value = 0.0f;

            clipLengthTween = DOTween.To(() => playTime.Value,
                num => playTime.Value = num,
                1.0f,
                length
                ).SetEase(Ease.Linear);
        }
    }
}
