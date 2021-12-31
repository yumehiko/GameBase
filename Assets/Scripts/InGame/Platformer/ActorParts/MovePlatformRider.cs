using System;
using UnityEngine;
using UniRx;

namespace yumehiko.Platformer
{
    /// <summary>
    /// 動く床に乗って、動く床から受ける速度を保持する。
    /// </summary>
    [Serializable]
    public class MovePlatformRider : IMovePlatformRider, IDisposable
    {
        /// <summary>
        /// 床から受ける速度。
        /// </summary>
        public Vector2 AdditionVelocity { get; private set; }

        [SerializeField] private Rigidbody2D body;
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
            //Y軸のうち、上昇分は無視（跳ねまくるので）。
            force.y = force.y > 0.0f ? 0.0f : force.y;

            AdditionVelocity = force;
        }
    }
}
