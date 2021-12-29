using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Spine.Unity;
using DG.Tweening;
using yumehiko.Resident;
using yumehiko.Resident.Control;

namespace yumehiko.Platformer
{
    /// <summary>
    /// プラットフォーマーゲームのプレイヤーキャラクター。
    /// </summary>
    public class PlatformerPlayer : MonoBehaviour, IContorlable, IDieable, IPlayerAction, IRideable
    {
        public ReadOnlyReactiveProperty<bool> IsDied => isDied.ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<bool> OnAction => onAction.ToReadOnlyReactiveProperty();
        public bool CanControl { get; private set; } = true;

        [SerializeField] private Walk2D walk;
        [SerializeField] private ActorAnimation actorAnimation;
        [Space]

        private bool isInvisible = false;
        private BoolReactiveProperty isDied = new BoolReactiveProperty(false);
        private BoolReactiveProperty onAction = new BoolReactiveProperty(false);



        private void Awake()
        {
            walk.Awake();
            actorAnimation.Awake(this, walk, walk);

            SubscribeKeys();

            PauseManager.IsPause.Where(isOn => !isOn)
                .Skip(1)
                .Subscribe(_ => OnResumePause())
                .AddTo(this);
        }

        private void OnDestroy()
        {
            walk.Dispose();
            actorAnimation.Dispose();
        }



        public void SetCanControl(bool isOn)
        {
            if (isDied.Value)
            {
                return;
            }

            CanControl = isOn;
            walk.Stop();
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

            CanControl = false;
            walk.Stop(true);
            isDied.Value = true;
        }

        public void SetRiderVelocity(Vector2 velocity) => walk.SetRiderVelocity(velocity);



        private void SubscribeKeys()
        {
            ReactiveInput.OnPeke
                .Where(_ => CanControl)
                .Where(_ => !PauseManager.IsPause.Value)
                .Where(_ => !(ReactiveInput.OnMove.Value.y <= -0.9f))
                .Where(isOn => isOn)
                .Subscribe(_ => walk.Jump())
                .AddTo(this);

            ReactiveInput.OnPeke
                .Where(_ => CanControl)
                .Where(_ => !PauseManager.IsPause.Value)
                .Where(isOn => !isOn)
                .Subscribe(_ => walk.CancelJump())
                .AddTo(this);

            ReactiveInput.OnPeke
                .Where(_ => CanControl)
                .Where(_ => !PauseManager.IsPause.Value)
                .Where(_ => ReactiveInput.OnMove.Value.y <= -0.9f)
                .Where(isOn => isOn)
                .Subscribe(_ => walk.Grounded.DownPlatform())
                .AddTo(this);

            ReactiveInput.OnMaru
                .Where(_ => CanControl)
                .Where(_ => !PauseManager.IsPause.Value)
                .Subscribe(isOn => onAction.Value = isOn)
                .AddTo(this);

            ReactiveInput.OnMove
                .Where(_ => CanControl)
                .Where(_ => !PauseManager.IsPause.Value)
                .Subscribe(axis => walk.Move(axis))
                .AddTo(this);

            ReactiveInput.OnRestart
                .Where(_ => CanControl)
                .Where(_ => !PauseManager.IsPause.Value)
                .Where(isOn => isOn)
                .Subscribe(_ => Die())
                .AddTo(this);
        }

        /// <summary>
        /// ポーズを解除したとき、その時のコントローラの状態を反映する。
        /// </summary>
        private void OnResumePause()
        {
            walk.Move(ReactiveInput.OnMove.Value);
            if (!ReactiveInput.OnPeke.Value)
            {
                walk.CancelJump();
            }
        }
    }
}