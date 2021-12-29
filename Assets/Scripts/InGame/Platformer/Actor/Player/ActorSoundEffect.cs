using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Platformer
{
    /// <summary>
    /// プラットフォーマーの基本的なキャラクターの効果音。
    /// </summary>
    [System.Serializable]
    public class ActorSoundEffect
    {
        private AudioSource audioSource;
        private AudioClip deathClip;
        private AudioClip jumpClip;
        private List<AudioClip> footStepClips;

        public ActorSoundEffect(AudioSource audioSource, AudioClip deathClip, AudioClip jumpClip, List<AudioClip> footStepClips)
        {
            this.audioSource = audioSource;
            this.deathClip = deathClip;
            this.jumpClip = jumpClip;
            this.footStepClips = footStepClips;
        }

        public void Death()
        {
            audioSource.volume = 1.0f;
            audioSource.pitch = 1.0f;
            audioSource.PlayOneShot(deathClip);
        }

        public void Jump()
        {
            audioSource.volume = 0.25f;
            audioSource.pitch = Random.Range(0.8f, 1.25f);
            audioSource.PlayOneShot(jumpClip);
        }

        public void FootStep(float volume = 0.5f)
        {
            audioSource.volume = volume;

            float randomPitch = Random.Range(0.75f, 1.25f);
            audioSource.pitch = randomPitch;

            int pickID = Random.Range(0, footStepClips.Count);
            audioSource.PlayOneShot(footStepClips[pickID]);
        }
    }
}