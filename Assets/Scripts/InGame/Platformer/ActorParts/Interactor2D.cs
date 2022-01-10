using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace yumehiko.Platformer
{
    /// <summary>
    /// インタラクトする者。
    /// </summary>
    [Serializable]
    public class Interactor2D : IInteractor, IDisposable
    {
        [SerializeField] private Collider2D bodyCollider;

        private List<IInteractable> interactables = new List<IInteractable>();
        private CompositeDisposable disposables;

        public void Initialize()
        {
            disposables = new CompositeDisposable();

            bodyCollider.OnTriggerEnter2DAsObservable()
                .Subscribe(interactable => TryEnterInteractable(interactable))
                .AddTo(disposables);

            bodyCollider.OnTriggerExit2DAsObservable()
                .Subscribe(interactable => TryExitInteractable(interactable))
                .AddTo(disposables);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        /// <summary>
        /// 現在触れている全てのInteractableにインタラクトする。
        /// </summary>
        public void TryInteract()
        {
            foreach (var touch in interactables)
            {
                touch.Interact(this);
            }
        }


        /// <summary>
        /// Interactableに触れる。
        /// </summary>
        /// <param name="interactable"></param>
        private void TryEnterInteractable(Collider2D target)
        {
            IInteractable interactable = target.GetComponent<IInteractable>();
            if(interactable == null)
            {
                return;
            }

            interactable.TouchEnter(this);
            interactables.Add(interactable);
        }

        /// <summary>
        /// Interactableをリストから削除。
        /// </summary>
        /// <param name="interactable"></param>
        private void TryExitInteractable(Collider2D target)
        {
            IInteractable interactable = target.GetComponent<IInteractable>();
            if (interactable == null)
            {
                return;
            }

            interactable.TouchExit(this);
            interactables.Remove(interactable);
        }
    }
}
