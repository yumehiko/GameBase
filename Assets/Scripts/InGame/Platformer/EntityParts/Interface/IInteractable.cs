using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Platformer
{
    /// <summary>
    /// インタラクト性。インタラクトされる側。
    /// </summary>
    public interface IInteractable : ITouchable
    {
        IObservable<IInteractor> OnInteract { get; }
        void Interact(IInteractor interactor);
    }
}
