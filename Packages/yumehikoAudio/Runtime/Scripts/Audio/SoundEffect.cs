using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.Audio
{
    /// <summary>
    /// 効果音を再生する。
    /// </summary>
    public class SoundEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        /// <summary>
        /// 指定した効果音を再生する。
        /// </summary>
        public void PlayClip(AudioClip clip, float volumeScale = 1.0f, float pitch = 1.0f)
        {
            audioSource.volume = volumeScale;
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// 効果音を再生する。複数の候補からランダムに選ぶ。
        /// </summary>
        public void PlayClip(List<AudioClip> clips, float volumeScale = 1.0f, float pitch = 1.0f)
        {
            AudioClip clip = clips[Random.Range(0, clips.Count)];
            PlayClip(clip, volumeScale, pitch);
        }
    }
}