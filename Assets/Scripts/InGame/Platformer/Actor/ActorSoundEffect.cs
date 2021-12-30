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
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip deathClip;
        [SerializeField] private AudioClip jumpClip;
        [SerializeField] private List<AudioClip> footStepClips;

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