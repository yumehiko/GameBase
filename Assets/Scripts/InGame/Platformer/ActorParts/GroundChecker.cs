using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using System;

namespace yumehiko.Platformer
{
    /// <summary>
    /// 接地判定。地面と触れているかを判定する。
    /// </summary>
    [Serializable]
    public class GroundChecker : IGrounded, IDisposable
    {
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Collider2D checkCollider;
        [SerializeField] private Collider2D footCollider;

        /// <summary>
        /// 地面に触れているか。
        /// </summary>
        public ReadOnlyReactiveProperty<bool> IsGrounded => isGrounded.ToReadOnlyReactiveProperty();

        /// <summary>
        /// 前フレームの落下速度。
        /// </summary>
        public float FallSpeedOnLastFrame { get; private set; } = 0.0f;

        private int touchingGrounds = 0;
        private Tween coyoteTimeTween;
        private CompositeDisposable disposables;
        private BoolReactiveProperty isGrounded = new BoolReactiveProperty(false);



        public void Initialize()
        {
            disposables = new CompositeDisposable();

            //着地判定を設定。
            _ = checkCollider.OnTriggerEnter2DAsObservable()
                .Subscribe(_ => EnterGround())
                .AddTo(disposables);

            //離陸判定を設定。
            _ = checkCollider.OnTriggerExit2DAsObservable()
                .Subscribe(_ => ExitGround())
                .AddTo(disposables);

            //落下速度を監視。
            _ = Observable.EveryEndOfFrame()
                .Subscribe(_ => FallSpeedOnLastFrame = body.velocity.y)
                .AddTo(disposables);
        }

        public void Dispose()
        {
            coyoteTimeTween?.Kill();
            disposables.Dispose();
        }

        /// <summary>
        /// 降りれるタイプの足場を降りる。
        /// </summary>
        public void DownPlatform()
        {
            const float fallDuration = 0.3f;
            footCollider.enabled = false;
            _ = DOVirtual.DelayedCall(fallDuration, () => footCollider.enabled = true, false)
                .SetLink(footCollider.gameObject);
        }



        /// <summary>
        /// 地面に触れたとき。
        /// </summary>
        /// <param name="collision"></param>
        private void EnterGround()
        {
            touchingGrounds++;
            if (!isGrounded.Value)
            {
                coyoteTimeTween?.Kill();
                isGrounded.Value = true;
            }
        }

        /// <summary>
        /// 地面から離れたとき。
        /// </summary>
        /// <param name="collision"></param>
        private void ExitGround()
        {
            touchingGrounds--;
            if (touchingGrounds <= 0)
            {
                touchingGrounds = 0;
                CoyoteTime();
            }
        }

        /// <summary>
        /// 地面から離れた判定をほんの少しだけ遅らせる。
        /// 操作補助のための余地。
        /// </summary>
        private void CoyoteTime()
        {
            const float duration = 0.10f;
            coyoteTimeTween = DOVirtual.DelayedCall(duration, () => OffGround(), false);
        }

        /// <summary>
        /// 地面から離れる。
        /// </summary>
        private void OffGround()
        {
            if (touchingGrounds > 0)
            {
                return;
            }

            isGrounded.Value = false;
        }
    }
}