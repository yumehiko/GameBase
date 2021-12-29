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
        ReadOnlyReactiveProperty<bool> IsDied { get; }
        void Die();
    }
}