using System;
using UniRx;

namespace yumehiko.Platformer
{
    public interface IJumpable
    {
        IObservable<Unit> OnJump { get; }
        IObservable<Unit> OnFallWhileJump { get; }
        void Jump();
    }
}