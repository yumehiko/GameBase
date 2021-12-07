using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Resident
{
    /// <summary>
    /// ゲームを一時停止する。
    /// </summary>
    public static class PauseManager
    {
        private static BoolReactiveProperty canPause = new BoolReactiveProperty(true);
        private static BoolReactiveProperty isPause = new BoolReactiveProperty(false);

        /// <summary>
        /// ポーズ可能か。
        /// </summary>
        public static ReadOnlyReactiveProperty<bool> CanPause => canPause.ToReadOnlyReactiveProperty();

        /// <summary>
        /// ポーズ中か。
        /// </summary>
        public static ReadOnlyReactiveProperty<bool> IsPause => isPause.ToReadOnlyReactiveProperty();



        static PauseManager()
        {
            LoadManager.OnLoadWaitStart.Subscribe(_ => ForbidPause());
            LoadManager.OnLoadComplete.Subscribe(_ => PermitPause());
        }

        /// <summary>
        /// ポーズ状態を切り替える。
        /// </summary>
        public static void SwitchPause()
        {
            if(isPause.Value)
            {
                Resume();
                return;
            }
            Pause();
        }

        /// <summary>
        /// ポーズする。
        /// </summary>
        public static void Pause()
        {
            if(IsPause.Value)
            {
                return;
            }

            Time.timeScale = 0.0f;
            isPause.Value = true;
        }

        /// <summary>
        /// ポーズを解除。
        /// </summary>
        public static void Resume()
        {
            if (!IsPause.Value)
            {
                return;
            }

            Time.timeScale = 1.0f;
            isPause.Value = false;
        }

        /// <summary>
        /// ポーズを禁止。
        /// </summary>
        public static void ForbidPause()
        {
            Resume();
            canPause.Value = false;
        }

        /// <summary>
        /// ポーズを許可。
        /// </summary>
        public static void PermitPause()
        {
            canPause.Value = true;
        }
    }
}