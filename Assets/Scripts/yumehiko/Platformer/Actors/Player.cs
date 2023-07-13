using UniRx;
using UnityEngine;
using yumehiko.Inputs;
using yumehiko.Platformer.Actors.Parts;
using yumehiko.Platformer.Buildings.Parts;
using yumehiko.Resident;

namespace yumehiko.Platformer.Actors
{
    /// <summary>
    /// プラットフォーマーゲームのプレイヤーキャラクター。
    /// </summary>
    public class Player : MonoBehaviour, IContorlable, IDieable, ILevelBeater, IMovePlatformRider
    {
        public IReadOnlyReactiveProperty<bool> IsDied => isDied;
        public bool CanControl { get; private set; } = true;
        public bool CanBeat => !isDied.Value;

        [SerializeField] private Walk2D walk;
        [SerializeField] private ActorAnimation actorAnimation;
        [SerializeField] private Interactor2D interactor;
        private bool isInvisible = false;
        private BoolReactiveProperty isDied = new BoolReactiveProperty(false);


        private void Awake()
        {
            walk.Initialize();
            actorAnimation.Initialize(this, walk, walk, walk.Grounded, walk.BodyDirection);
            interactor.Initialize();

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
            interactor.Dispose();
        }


        public void SetCanControl(bool canControl)
        {
            if (isDied.Value)
            {
                return;
            }

            CanControl = canControl;
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

        public void Ride(IMovePlatform platform) => walk.MovePlatformRider.Ride(platform);

        public void GetOff(IMovePlatform platform) => walk.MovePlatformRider.GetOff(platform);


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
                .Where(isOn => isOn)
                .Subscribe(_ => interactor.TryInteract())
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
        
        public void OnBeatLevel()
        {
            CanControl = false;
            walk.Stop();
        }
    }
}