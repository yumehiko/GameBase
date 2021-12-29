using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace yumehiko.Platformer
{
    /// <summary>
    /// 二次元平面上を動く。強制的に移動させることもできる。
    /// </summary>
    public interface IMovable
    {
        ReadOnlyReactiveProperty<float> OnMove { get; }
        ReadOnlyReactiveProperty<ActorDirection> BodyDirection { get; }
        IGrounded Grounded { get; }

        void Move(Vector2 direction);
        void Stop(bool yIsZero = false);
    }
}