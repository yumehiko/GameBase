﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using DG.Tweening;
using Spine.Unity;

namespace yumehiko.Platformer
{
    [Serializable]
    public class ActorAnimation : IDisposable
    {
        [SerializeField] private SkeletonAnimation visual;
        [SerializeField] private ParticleSystem thiunParticle;
        [Space]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip deathClip;
        [SerializeField] private AudioClip jumpClip;
        [SerializeField] private List<AudioClip> footStepClips;

        private ActorSoundEffect soundEffect;
        private IGrounded grounded;
        private Sequence groundedSequence;
        private CompositeDisposable disposables;



        public void Awake(IDieable dieable, IMovable movable, IJumpable jumpable)
        {
            soundEffect = new ActorSoundEffect(audioSource, deathClip, jumpClip, footStepClips);
            grounded = movable.Grounded;
            //SkeletonAnimationはAwakeのタイミングでは購読できないので、後に回す。
            Observable.NextFrame().Subscribe(_ => SubscribeEvents(dieable, movable, jumpable));
        }

        public void Dispose()
        {
            visual.state.Event -= OnEvent;
            disposables.Dispose();
        }



        private void SubscribeEvents(IDieable dieable, IMovable movable, IJumpable jumpable)
        {
            disposables = new CompositeDisposable();

            //死亡時アニメーション。
            dieable.IsDied
                .Where(isOn => isOn)
                .Subscribe(_ => DeadAnimation())
                .AddTo(disposables);

            //アイドル時アニメーション。
            movable.OnMove
                .Where(_ => !dieable.IsDied.Value)
                .Where(vector => vector == 0.0f)
                .Subscribe(_ => IdleAnimation())
                .AddTo(disposables);

            //移動時アニメーション。
            movable.OnMove
                .Where(_ => !dieable.IsDied.Value)
                .Where(vector => vector != 0.0f)
                .Subscribe(_ => MoveAnimation())
                .AddTo(disposables);

            //ジャンプ時アニメーション
            jumpable.OnJump
                .Where(_ => !dieable.IsDied.Value)
                .Subscribe(_ => JumpAnimation())
                .AddTo(disposables);

            //着地時アニメーション。
            grounded.IsGrounded
                .Where(_ => !dieable.IsDied.Value)
                .Subscribe(_ => GroundedAnimation(grounded.FallSpeedOnLastFrame))
                .AddTo(disposables);

            //スケルトンの向き。
            movable.BodyDirection
                .Subscribe(direction => SetSkeletonDirection(direction))
                .AddTo(disposables);

            //フットステップイベント
            visual.state.Event += OnEvent;
        }

        private void OnEvent(Spine.TrackEntry entry, Spine.Event e)
        {
            switch(e.Data.Name)
            {
                case "FootStep":
                    FootStepEvent();
                    break;

                default:
                    break;
            }
        }

        private void FootStepEvent()
        {
            if (!grounded.IsGrounded.Value)
            {
                return;
            }
            soundEffect.FootStep();
        }

        private void IdleAnimation()
        {
            visual.state.ClearTrack(0);
            visual.skeleton.SetToSetupPose();
            visual.state.SetAnimation(0, "Stand", true);
        }

        private void MoveAnimation()
        {
            visual.state.ClearTrack(0);
            visual.skeleton.SetToSetupPose();
            visual.state.SetAnimation(0, "Walk", true);
        }

        private void JumpAnimation()
        {
            soundEffect.Jump();
        }

        private void SetSkeletonDirection(ActorDirection direction)
        {
            visual.skeleton.ScaleX = direction == ActorDirection.Right ? 1.0f : -1.0f;
        }

        private void GroundedAnimation(float fallSpeed)
        {
            if (fallSpeed > -3.0f)
            {
                return; 
            }

            soundEffect.FootStep(0.8f);

            groundedSequence?.Complete();
            Vector2 pesyanko = new Vector2(1.5f, 0.5f);

            groundedSequence = DOTween.Sequence();
            groundedSequence.Append(visual.transform.DOScale(pesyanko, 0.125f));
            groundedSequence.Append(visual.transform.DOScale(Vector2.one, 0.25f).SetEase(Ease.InQuad));
        }

        private void DeadAnimation()
        {
            visual.state.ClearTrack(0);
            visual.skeleton.SetToSetupPose();
            visual.state.SetAnimation(0, "Dead", true);

            soundEffect.Death();

            thiunParticle.Play();
        }
    }
}