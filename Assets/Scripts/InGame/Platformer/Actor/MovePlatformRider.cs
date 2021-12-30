using System;
using UnityEngine;
using UniRx;

namespace yumehiko.Platformer
{
    /// <summary>
    /// 動く床に乗って、動く床から受ける速度を保持する。
    /// </summary>
    public class MovePlatformRider : IMovePlatformRider, IDisposable
    {
        public Vector2 AdditionVelocity { get; private set; }


        private IDisposable platformObserver;
        private IMovePlatform ridingPlatform;


        public void Ride(IMovePlatform platform)
        {
            platformObserver?.Dispose();
            ridingPlatform = platform;
            platformObserver = platform.VelocityToRider.Subscribe(force => AdjustVelocity(force));
        }

        public void GetOff(IMovePlatform platform)
        {
            if (ridingPlatform != platform)
            {
                return;
            }

            ridingPlatform = null;
            platformObserver?.Dispose();
            AdditionVelocity = Vector2.zero;
        }

        public void Dispose()
        {
            platformObserver?.Dispose();
        }


        private void AdjustVelocity(Vector2 force)
        {
            //Y軸は無視（単に乗っかる）。
            force.y = 0.0f;

            AdditionVelocity = force;
        }
    }
}
