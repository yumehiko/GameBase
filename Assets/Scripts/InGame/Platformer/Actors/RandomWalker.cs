using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

namespace yumehiko.Platformer
{
    /// <summary>
    /// ランダムウォーカー。ランダムに移動したりジャンプしたりする。
    /// </summary>
    public class RandomWalker : MonoBehaviour, IDieable, IMovePlatformRider
    {
        public IReadOnlyReactiveProperty<bool> IsDied => isDied;

        [SerializeField] private Walk2D walk;
        [SerializeField] private ActorAnimation actorAnimation;
        private bool isInvisible = false;
        private BoolReactiveProperty isDied = new BoolReactiveProperty(false);
        private readonly float randomActDuration = 1.0f;
        private Tween actTween;

        private enum RandomAct
        {
            None,
            WalkRight,
            WalkLeft,
            Jump
        }

        private void Awake()
        {
            walk.Initialize();
            actorAnimation.Initialize(this, walk, walk, walk.Grounded, walk.BodyDirection);
            actTween = DOVirtual.DelayedCall(randomActDuration, () => RandomWalk(), false)
                .SetLoops(-1)
                .SetLink(gameObject);
        }

        private void OnDestroy()
        {
            walk.Dispose();
            actorAnimation.Dispose();
        }

        /// <summary>
        /// ランダムに移動する。
        /// </summary>
        private void RandomWalk()
        {
            RandomAct act = (RandomAct)Random.Range(0, 4);

            switch (act)
            {
                case RandomAct.None:
                    walk.Stop();
                    break;
                case RandomAct.WalkRight:
                    walk.Move(Vector2.right);
                    break;
                case RandomAct.WalkLeft:
                    walk.Move(Vector2.left);
                    break;
                case RandomAct.Jump:
                    walk.Jump();
                    break;
            }
        }

        public void Die()
        {
            if (isDied.Value)
            {
                return;
            }

            if (isInvisible)
            {
                return;
            }

            actTween?.Kill();
            walk.Stop(true);
            isDied.Value = true;
        }

        public void Ride(IMovePlatform platform) => walk.MovePlatformRider.Ride(platform);

        public void GetOff(IMovePlatform platform) => walk.MovePlatformRider.GetOff(platform);
    }
}
