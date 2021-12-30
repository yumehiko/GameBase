using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Platformer
{
    /// <summary>
    /// 動く床に乗って、速度の影響を受ける。
    /// </summary>
    public interface IMovePlatformRider
    {
        /// <summary>
        /// 指定した足場に乗る。
        /// </summary>
        void Ride(IMovePlatform platform);

        /// <summary>
        /// 足場から降りる。
        /// </summary>
        void GetOff(IMovePlatform platform);
    }
}
