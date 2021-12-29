using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yumehiko.Audio.Music
{
    /// <summary>
    /// 指定したClipを再生してもらう。
    /// </summary>
    public class MusicDisc : MonoBehaviour
    {
        [SerializeField] private MusicClip clip = default;
        [SerializeField] private bool initPlay = false;
        [SerializeField] private bool doLoop = true;

        private void Start()
        {
            if (initPlay)
            {
                PlayDisc();
            }
        }

        /// <summary>
        /// 指定されているBGM Clipを再生する。
        /// </summary>
        /// <param name="duration"></param>
        public void PlayDisc(float duration = 1.0f)
        {
            if (clip == null)
            {
                return;
            }

            MusicManager.PlayMusic(clip, duration);
            MusicManager.SetLoopMode(doLoop);
        }
    }
}
