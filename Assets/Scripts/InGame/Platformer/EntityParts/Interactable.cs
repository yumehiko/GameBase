using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Platformer
{
    /// <summary>
    /// 接触とインタラクトが可能。
    /// </summary>
    public class Interactable : MonoBehaviour, IInteractable
    {
        public System.IObservable<IInteractor> OnTouchEnter => onTouchEnter;
        public System.IObservable<IInteractor> OnTouchExit => onTouchExit;
        public System.IObservable<IInteractor> OnInteract => onInteract;

        private Subject<IInteractor> onTouchEnter = new Subject<IInteractor>();
        private Subject<IInteractor> onTouchExit = new Subject<IInteractor>();
        private Subject<IInteractor> onInteract = new Subject<IInteractor>();

        public void TouchEnter(IInteractor toucher) => onTouchEnter.OnNext(toucher);
        public void TouchExit(IInteractor toucher) => onTouchExit.OnNext(toucher);
        public void Interact(IInteractor interactor) => onInteract.OnNext(interactor);
    }
}
