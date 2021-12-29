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

        private void Start()
        {
            //親が存在するなら、ロード時に破壊するかは親が決める。
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }

            FadeIn(0.5f);
            LoadManager.OnLoadWaitStart.Subscribe(duration => FadeOut(duration)).AddTo(this);
            LoadManager.OnLoadComplete.Subscribe(duration => FadeIn(duration)).AddTo(this);
        }

        /// <summary>
        /// フェードアウト（徐々に暗くなり、見えなくなる）。
        /// </summary>
        /// <param name="duration"></param>
        private void FadeOut(float duration)
        {
            transition?.Kill();

            transition = DOTween.Sequence();
            transition.OnStart(() => kuroKoma.enabled = true);
            transition.Append(kuroKoma.DOFade(1.0f, duration));
            transition.SetUpdate(true);
            transition.SetLink(gameObject);
        }

        /// <summary>
        /// フェードイン（徐々に明るくなり、見えるようになる）。
        /// </summary>
        /// <param name="duration"></param>
        private void FadeIn(float duration)
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