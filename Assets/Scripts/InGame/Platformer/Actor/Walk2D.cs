using UnityEngine;
using UniRx;
using System;

namespace yumehiko.Platformer
{
    /// <summary>
    /// 重力に影響され、地上を左右に歩く。ジャンプする。
    /// </summary>
    [Serializable]
    public class Walk2D : IMovable, IJumpable, IRideable, IDisposable
    {
        public ReadOnlyReactiveProperty<float> OnMove => onMove.ToReadOnlyReactiveProperty();
        public IObservable<Unit> OnJump => onJump;
        public IObservable<Unit> OnFallWhileJump => onFallWhileJump;
        public ReadOnlyReactiveProperty<ActorDirection> BodyDirection => bodyDirection.ToReadOnlyReactiveProperty();
        public IGrounded Grounded => groundChecker;


        [SerializeField] private Rigidbody2D body;
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private GroundChecker groundChecker;
        private Vector2 additionVelocity = Vector2.zero;
        private FloatReactiveProperty onMove = new FloatReactiveProperty(0.0f);
        private Subject<Unit> onJump = new Subject<Unit>();
        private Subject<Unit> onFallWhileJump = new Subject<Unit>();
        private ReactiveProperty<ActorDirection> bodyDirection = new ReactiveProperty<ActorDirection>(ActorDirection.Right);
        private CompositeDisposable disposables;


        public void Awake()
        {
            groundChecker.Awake(body);

            disposables = new CompositeDisposable();

            //ボディに速度を適応する。
            _ = Observable.EveryFixedUpdate()
                .Subscribe(_ => UpdateVelocity())
                .AddTo(disposables);

            //初期状態で、重力加速度を最大値で当てはめておく。
            body.velocity = Physics2D.gravity;
        }

        public void Dispose()
        {
            groundChecker.Dispose();
            disposables.Dispose();
        }

        /// <summary>
        /// 横方向に歩く。
        /// </summary>
        /// <param name="direction">移動方向。x成分のみを評価し、正なら右方向へ。</param>
        /// <param name="speed">移動速度。</param>
        public void Move(Vector2 direction)
        {
            var walkSpeed = direction.x * speed;
            onMove.Value = walkSpeed;

            //体の向きを反映。
            if (direction.x == 0.0f)
            {
                return;
            }
            bodyDirection.Value = direction.x >= 0.0f ? ActorDirection.Right : ActorDirection.Left;
        }

        /// <summary>
        /// 移動を中止する。
        /// </summary>
        public void Stop(bool yIsZero = false)
        {
            body.velocity = yIsZero ? Vector2.zero : new Vector2(0.0f, body.velocity.y);
            onMove.Value = 0.0f;
        }

        /// <summary>
        /// ジャンプする。
        /// </summary>
        /// <param name="jumpForce">ジャンプ力。</param>
        public void Jump()
        {
            if (!groundChecker.IsGrounded.Value)
            {
                return;
            }

            body.velocity = new Vector2(body.velocity.x, 0.0f);
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            onJump.OnNext(Unit.Default);

            //ジャンプ開始時に一度だけ、落下開始を監視する。
            _ = body.ObserveEveryValueChanged(_ => body.velocity.y)
                .Where(yVelocity => yVelocity <= 0.0f)
                .First()
                .Subscribe(_ => onFallWhileJump.OnNext(Unit.Default))
                .AddTo(disposables);
        }

        /// <summary>
        /// ジャンプ中に、ジャンプをキャンセルする。
        /// </summary>
        public void CancelJump()
        {
            if (body.velocity.y < 0.0f)
            {
                return;
            }

            const float jumpBrakeFactor = 0.5f;
            float yVelocity = body.velocity.y * jumpBrakeFactor;
            body.velocity = new Vector2(body.velocity.x, yVelocity);
        }

        /// <summary>
        /// 移動床によって速度を調整する。
        /// </summary>
        /// <param name="velocity"></param>
        public void SetRiderVelocity(Vector2 velocity)
        {
            velocity.y = velocity.y > 0.0f ? 0.0f : velocity.y;
            SetAdditionVelocity(velocity);
        }

        /// <summary>
        /// 追加の加速度をセットする。
        /// </summary>
        /// <param name="velocity"></param>
        public void SetAdditionVelocity(Vector2 velocity)
        {
            additionVelocity = velocity;
        }


        private void UpdateVelocity()
        {
            body.velocity = new Vector2(onMove.Value, body.velocity.y) + additionVelocity;
        }
    }
}