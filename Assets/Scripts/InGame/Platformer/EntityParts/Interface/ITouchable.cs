using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Platformer
{
    /// <summary>
    /// 接触性。
    /// </summary>
    public interface ITouchable
    {
        System.IObservable<IInteractor> OnTouchEnter { get; }
        System.IObservable<IInteractor> OnTouchExit { get; }
        void TouchEnter(IInteractor toucher);
        void TouchExit(IInteractor toucher);
    }
}
