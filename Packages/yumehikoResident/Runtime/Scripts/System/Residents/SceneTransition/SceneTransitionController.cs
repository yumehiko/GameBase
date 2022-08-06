using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

namespace yumehiko.Resident
{
    /// <summary>
    /// シーン間遷移時のアニメーション（フェードアウトなど）
    /// </summary>
    public class SceneTransitionController : MonoBehaviour
    {
        [SerializeField] private FadeTransition defaultTransition;
        private ITransition transition;

        private void Start()
        {
            //親が存在するなら、ロード時に破壊するかは親が決める。
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }

            transition = defaultTransition;

            _ = LoadManager.OnLoadTransitionStart.Subscribe(duration => transition.Begin(duration)).AddTo(this);
            _ = LoadManager.OnLoadComplete.Subscribe(duration => transition.End(duration)).AddTo(this);
        }

        /// <summary>
        /// 遷移アニメーションを指定する。
        /// </summary>
        /// <param name="transition"></param>
        public void SetTransitionAnimation(ITransition transition)
        {
            this.transition = transition;
        }
    }
}