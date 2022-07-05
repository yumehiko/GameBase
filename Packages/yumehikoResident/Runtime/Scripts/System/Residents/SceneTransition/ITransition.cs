using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.Resident
{
    /// <summary>
    /// 遷移アニメーション。
    /// </summary>
    public interface ITransition
    {
        /// <summary>
		/// 遷移を開始する。
		/// </summary>
		/// <param name="duration"></param>
        void Begin(float duration);

        /// <summary>
		/// 遷移を終了する。
		/// </summary>
		/// <param name="duration"></param>
        void End(float duration);
    }
}
