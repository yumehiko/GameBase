using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using yumehiko.Resident;

namespace yumehiko.UI
{

    /// <summary>
    /// シーン間遷移時のアニメーション（フェードアウトなど）
    /// </summary>
    public class SceneTransitionAnimation : MonoBehaviour
    {
        //TODO: 黒駒以外の遷移を追加するなら、enumが必要だろう。

        [SerializeField] private Image kuroKoma;

        private Sequence transition;

        private void Awake()
        {
            LoadManager.OnLoadWaitStart.Subscribe(duration => FadeOut(duration)).AddTo(this);
            LoadManager.OnLoadComplete.Subscribe(duration => FadeIn(duration)).AddTo(this);
        }

        private void FadeOut(float duration)
        {
            transition?.Kill();

            transition = DOTween.Sequence();
            transition.OnStart(() => kuroKoma.enabled = true);
            transition.Append(kuroKoma.DOFade(1.0f, duration));
            transition.SetLink(gameObject);
        }

        private void FadeIn(float duration)
        {
            transition?.Kill();

            transition = DOTween.Sequence();
            transition.Append(kuroKoma.DOFade(1.0f, duration));
            transition.OnComplete(() => kuroKoma.enabled = false);
            transition.SetLink(gameObject);
        }
    }

}