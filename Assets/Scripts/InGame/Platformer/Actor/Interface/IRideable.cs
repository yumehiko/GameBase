using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Platformer
{
    /// <summary>
    /// 動く床などに乗れる。
    /// </summary>
    public interface IRideable
    {
        /// <summary>
        /// 床から影響する速度をセット。
        /// </summary>
        /// <param name="velocity"></param>
        void SetRiderVelocity(Vector2 velocity);
    }
}
