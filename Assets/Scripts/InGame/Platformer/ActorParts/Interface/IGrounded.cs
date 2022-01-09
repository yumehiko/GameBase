using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Platformer
{
    /// <summary>
    /// 接地判定。
    /// </summary>
    public interface IGrounded
    {
        IReadOnlyReactiveProperty<bool> IsGrounded { get; }
        float FallSpeedOnLastFrame { get; }

        /// <summary>
        /// 降りれるタイプの足場を降りる。
        /// </summary>
        void DownPlatform();
    }
}