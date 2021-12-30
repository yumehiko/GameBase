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
        ReadOnlyReactiveProperty<Vector2> VelocityToRider { get; }
    }
}
