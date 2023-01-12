using UnityEngine;
using DG.Tweening;
using UniRx;
using yumehiko.Resident;
using Cysharp.Threading.Tasks;
using System.Threading;

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

        /// <summary>
        /// CombineAudioSourceをインストール済みか。
        /// </summary>
        public static bool IsInstalled => combineAudioSource != null;

        private static CombineAudioSource combineAudioSource;

        private static readonly Subject<MusicClip> onStartMusic = new Subject<MusicClip>();
        private static readonly Subject<Unit> onStopMusic = new Subject<Unit>();
        private static readonly BoolReactiveProperty isPausing = new BoolReactiveProperty();
        private static readonly FloatReactiveProperty playTime = new FloatReactiveProperty(0.0f);
        private static CompositeDisposable pauseModeDisposables;

        private static Tweener clipLengthTween;

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
            MusicManager.combineAudioSource = combineAudioSource;
            _ = onStartMusic.Subscribe(clip => MeasureClipTime(clip));
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
            if (!combineAudioSource.IsPlaying)
            {
                CurrentClip = musicClip;
                onStartMusic.OnNext(musicClip);
                combineAudioSource.InstantSwap(musicClip);
                return;
            }

            //指定時間かけて、クロスフェードする。
            CurrentClip = musicClip;
            onStartMusic.OnNext(musicClip);
            combineAudioSource.CrossFadeSwap(musicClip, duration).Forget();
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

            combineAudioSource.StopMusic(fadeOutDuration).Forget();
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
            combineAudioSource.SetLoopMode(doLoop);
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
                _ = PauseManager.IsPause
                    .Subscribe(isPause => SwitchPause(isPause))
                    .AddTo(pauseModeDisposables);
            }
            else
            {
                SwitchPause(false);
                pauseModeDisposables?.Dispose();
            }
        }

        /// <summary>
        /// 一時停止を切り替える。
        /// </summary>
        private static void SwitchPause(bool isPause)
        {
            combineAudioSource.SwitchPause(isPause);
            isPausing.Value = isPause;
        }

        /// <summary>
        /// クリップを入れ替えたとき、そのクリップの長さを測る。
        /// </summary>
        /// <param name="musicClip"></param>
        private static void MeasureClipTime(MusicClip musicClip)
        {
            float length = musicClip.Length;
            playTime.Value = 0.0f;
            clipLengthTween?.Kill();

            clipLengthTween = DOTween.To(() => playTime.Value,
                num => playTime.Value = num,
                1.0f,
                length
                ).SetEase(Ease.Linear);
        }
    }
}
