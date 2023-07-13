using System;
using yumehiko.Platformer.Actors.Parts;

namespace yumehiko.Platformer.Entity.Parts
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
