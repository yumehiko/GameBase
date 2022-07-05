using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace yumehiko.Resident
{
    public class FadeTransition : MonoBehaviour, ITransition
    {
        [SerializeField] private Image kuroKoma;

        private Sequence transition;

        public void Begin(float duration)
        {
            transition?.Kill();

            transition = DOTween.Sequence();
            transition.OnStart(() => kuroKoma.enabled = true);
            transition.Append(kuroKoma.DOFade(1.0f, duration));
            transition.SetUpdate(true);
            transition.SetLink(gameObject);
        }

        public void End(float duration)
        {
            transition?.Kill();

            transition = DOTween.Sequence();
            transition.OnStart(() => kuroKoma.enabled = true);
            transition.Append(kuroKoma.DOFade(0.0f, duration));
            transition.OnComplete(() => kuroKoma.enabled = false);
            transition.SetUpdate(true);
            transition.SetLink(gameObject);
        }
    }
}
