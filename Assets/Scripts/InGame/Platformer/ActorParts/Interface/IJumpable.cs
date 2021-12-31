using System;
using UniRx;

namespace yumehiko.Platformer
{
    public interface IJumpable
    {
        /// <summary>
        /// ジャンプしたとき。
        /// </summary>
        IObservable<Unit> OnJump { get; }

        /// <summary>
        /// ジャンプ後、落下し始めたとき。
        /// </summary>
        IObservable<Unit> OnFallWhileJump { get; }

        /// <summary>
        /// ジャンプする。
        /// </summary>
        void Jump();
    }
}