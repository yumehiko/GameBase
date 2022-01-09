using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko
{
    /// <summary>
    /// 死亡の概念。
    /// </summary>
    public interface IDieable
    {
        /// <summary>
        /// 死亡したか。
        /// </summary>
        IReadOnlyReactiveProperty<bool> IsDied { get; }

        /// <summary>
        /// 死亡する。
        /// </summary>
        void Die();
    }
}