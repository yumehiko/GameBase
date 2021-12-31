using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko
{
    /// <summary>
    /// 動く足場。
    /// </summary>
    public interface IMovePlatform
    {
        /// <summary>
        /// 乗っているキャラクターに与える速度。
        /// </summary>
        ReadOnlyReactiveProperty<Vector2> VelocityToRider { get; }
    }
}
