using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace yumehiko.Platformer
{
    /// <summary>
    /// 二次元平面上を動く。
    /// </summary>
    public interface IMovable
    {
        /// <summary>
        /// 移動量が指定されたとき。その移動量。
        /// </summary>
        IReadOnlyReactiveProperty<float> OnMove { get; }

        /// <summary>
        /// 移動量を指定する。
        /// </summary>
        /// <param name="direction"></param>
        void Move(Vector2 direction);

        /// <summary>
        /// 停止する。
        /// </summary>
        /// <param name="yIsZero"></param>
        void Stop(bool yIsZero = false);
    }
}