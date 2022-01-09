using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UniRx;
using Cysharp.Threading.Tasks;

namespace yumehiko.Resident
{

    /// <summary>
    /// ゲームのロードを管理。
    /// </summary>
    public static class LoadManager
    {
        /// <summary>
        /// ロード準備開始。また、実際のロードを開始するまでの遅延。
        /// </summary>
        public static System.IObservable<float> OnLoadWaitStart => onLoadWaitStart;

        /// <summary>
        /// ロード完了時。また、ロード完了後からロードが明けたとするまでの時間。
        /// </summary>
        public static System.IObservable<float> OnLoadComplete => onLoadComplete;

        /// <summary>
        /// 現在ロード中か。
        /// </summary>
        public static bool IsSceneSwapping => loadTween.IsActive();

        /// <summary>
        /// 現在アクティブなシーンのID。
        /// </summary>
        public static int CurrentSceneID => SceneManager.GetActiveScene().buildIndex;


        private static Subject<float> onLoadWaitStart = new Subject<float>();
        private static Subject<float> onLoadComplete = new Subject<float>();
        private static Tween loadTween;


        /// <summary>
        /// シーンを再読み込み。
        /// </summary>
        /// <param name="delay">再読み込みまでの遅延。</param>
        /// <param name="endDelay">読み込み明けの遅延。</param>
        public static void RequireResetScene(float delay = 0.5f, float endDelay = 0.5f)
        {
            RequireLoadScene(SceneManager.GetActiveScene().buildIndex, delay, endDelay);
        }

        /// <summary>
        /// 指定したBuild IDのシーンをロードを要求する。
        /// </summary>
        /// <param name="delay">再読み込みまでの遅延。</param>
        /// <param name="endDelay">読み込み明けの遅延。</param>
        public static void RequireLoadScene(int buildID, float delay = 0.5f, float endDelay = 0.5f)
        {
            if (loadTween.IsActive())
            {
                return;
            }

            onLoadWaitStart.OnNext(delay);
            loadTween = DOVirtual.DelayedCall(delay, () => LoadScene(buildID, endDelay).Forget());
        }

        /// <summary>
        /// 指定した名前のシーンのロードを要求する。
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="delay"></param>
        /// <param name="endDelay"></param>
        public static void RequireLoadScene(string sceneName, float delay = 0.5f, float endDelay = 0.5f)
        {
            if (loadTween.IsActive())
            {
                return;
            }

            onLoadWaitStart.OnNext(delay);
            loadTween = DOVirtual.DelayedCall(delay, () => LoadScene(sceneName, endDelay).Forget());
        }

        /// <summary>
        /// シーンを実際にロードする。
        /// </summary>
        /// <param name="buildID">読み込むシーンのID。</param>
        /// <param name="endDelay">読み込み明けの遅延。</param>
        private static async UniTask LoadScene(int buildID, float endDelay)
        {
            await SceneManager.LoadSceneAsync(buildID);
            onLoadComplete.OnNext(endDelay);
        }

        /// <summary>
        /// シーンを実際にロードする。
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="endDelay"></param>
        private static async UniTask LoadScene(string sceneName, float endDelay)
        {
            await SceneManager.LoadSceneAsync(sceneName);
            onLoadComplete.OnNext(endDelay);
        }
    }

}