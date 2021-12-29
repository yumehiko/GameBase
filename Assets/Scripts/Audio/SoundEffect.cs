using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.Audio
{
    /// <summary>
    /// 効果音を再生する。
    /// TODO:機能が貧弱すぎて今のところいらないな。
    /// </summary>
    public class SoundEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        public void PlayClip(AudioClip clip, float volume = 1.0f, float pitch = 1.0f)
        {
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip);
        }
    }
}