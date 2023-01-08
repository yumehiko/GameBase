using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace yumehiko.Audio
{
    /// <summary>
    /// 効果音を再生する。
    /// </summary>
    public class SoundEffect : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> clips;

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
        /// 効果音を再生する。指定した複数の候補からランダムに選ぶ。
        /// </summary>
        public void PlayClip(List<AudioClip> clips, float volumeScale = 1.0f, float pitch = 1.0f)
        {
            AudioClip clip = clips[Random.Range(0, clips.Count)];
            PlayClip(clip, volumeScale, pitch);
        }

        /// <summary>
        /// 指定した効果音を再生し、再生が終えるまでawaitする。
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="volumeScale"></param>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public async UniTask PlayClipAsync(AudioClip clip, CancellationToken token, float volumeScale = 1.0f, float pitch = 1.0f)
        {
            audioSource.volume = volumeScale;
            audioSource.pitch = pitch;
            audioSource.clip = clip;
            audioSource.Play();
            await UniTask.WaitUntil(() => audioSource.isPlaying == true, cancellationToken: token);
            await UniTask.WaitUntil(() => audioSource.isPlaying == false, cancellationToken: token);

        }

        /// <summary>
        /// このコンポーネントが保持しているクリップをランダムに選ぶ。
        /// </summary>
        /// <param name="volumeScale"></param>
        /// <param name="pitch"></param>
        public void PlayOwnClip(float volumeScale = 1.0f, float pitch = 1.0f)
        {
            AudioClip clip = clips[Random.Range(0, clips.Count)];
            PlayClip(clip, volumeScale, pitch);
        }
    }
}