using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
        /// 画面遷移開始時。値は実際のロードまでの秒数。
        /// </summary>
        public static IObservable<float> OnLoadTransitionStart => onLoadTransitionStart;

        /// <summary>
        /// ロード完了時。値は画面遷移終了までの時間。
        /// MEMO：awaitやstartでは受け取れないっぽい。
        /// </summary>
        public static IObservable<float> OnLoadComplete => onLoadComplete;

        /// <summary>
        /// 画面遷移完了時。
        /// </summary>
        public static IObservable<Unit> OnLoadTransitionEnd => onLoadTransitionEnd;

        /// <summary>
        /// 現在ロード中か。
        /// </summary>
        public static bool IsSceneSwapping => isLoading;

        /// <summary>
        /// 現在アクティブなシーンのID。
        /// </summary>
        public static int CurrentSceneID => SceneManager.GetActiveScene().buildIndex;


        private static Subject<float> onLoadTransitionStart = new Subject<float>();
        private static Subject<float> onLoadComplete = new Subject<float>();
        private static Subject<Unit> onLoadTransitionEnd = new Subject<Unit>();

        private static bool isLoading = false;



        /// <summary>
        /// シーンを再読み込み。
        /// </summary>
        /// <param name="beginDelay">再読み込みまでの遅延。</param>
        /// <param name="endDelay">読み込み明けの遅延。</param>
        public static void RequireResetScene(float beginDelay = 0.5f, float endDelay = 0.5f)
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex, beginDelay, endDelay).Forget();
        }

        /// <summary>
        /// 指定したBuild IDのシーンをロードを要求する。
        /// </summary>
        /// <param name="beginDelay">再読み込みまでの遅延。</param>
        /// <param name="endDelay">読み込み明けの遅延。</param>
        public static void RequireLoadScene(int buildID, float beginDelay = 0.5f, float endDelay = 0.5f)
        {
            LoadScene(buildID, beginDelay, endDelay).Forget();
        }

        /// <summary>
        /// 指定した名前のシーンのロードを要求する。
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="beginDelay"></param>
        /// <param name="endDelay"></param>
        public static void RequireLoadScene(string sceneName, float beginDelay = 0.5f, float endDelay = 0.5f)
        {
            LoadScene(sceneName, beginDelay, endDelay).Forget();
        }

        /// <summary>
        /// シーンを実際にロードする。
        /// </summary>
        /// <param name="buildID">読み込むシーンのID。</param>
        /// <param name="endDelay">読み込み明けの遅延。</param>
        private static async UniTask LoadScene(int buildID, float beginDelay, float endDelay)
        {
            if (isLoading)
            {
                return;
            }

            isLoading = true;

            onLoadTransitionStart.OnNext(beginDelay);
            await UniTask.Delay(TimeSpan.FromSeconds(beginDelay));

            await SceneManager.LoadSceneAsync(buildID);
            onLoadComplete.OnNext(endDelay);

            await UniTask.Delay(TimeSpan.FromSeconds(endDelay));
            onLoadTransitionEnd.OnNext(Unit.Default);

            isLoading = false;
        }

        /// <summary>
        /// シーンを実際にロードする。
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="endDelay"></param>
        private static async UniTask LoadScene(string sceneName, float beginDelay, float endDelay)
        {
            if (isLoading)
            {
                return;
            }

            isLoading = true;

            onLoadTransitionStart.OnNext(beginDelay);
            await UniTask.Delay(TimeSpan.FromSeconds(beginDelay));

            await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
            onLoadComplete.OnNext(endDelay);

            await UniTask.Delay(TimeSpan.FromSeconds(endDelay));
            onLoadTransitionEnd.OnNext(Unit.Default);

            isLoading = false;
        }
    }

}