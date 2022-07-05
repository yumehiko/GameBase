using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace yumehiko.Audio.Music
{
    /// <summary>
    /// サウンドテスト。ボタンを押すとBGMを変更する。
    /// </summary>
    public class SoundTestJukeBox : MonoBehaviour
    {
        [SerializeField] private List<MusicClip> clips;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;
        private int index = 0;

        private void Awake()
        {
            nextButton.OnClickAsObservable().Subscribe(_ => PlayNext());
            prevButton.OnClickAsObservable().Subscribe(_ => PlayPrev());
            _ = Observable.NextFrame().Subscribe(_ =>
            {
                MusicManager.PlayMusic(clips[0]);
                MusicManager.SetLoopMode(true);
            });
        }

        private void PlayNext()
        {
            index++;
            if (index >= clips.Count)
            {
                index = 0;
            }

            MusicManager.PlayMusic(clips[index]);
        }

        private void PlayPrev()
        {
            index--;
            if (index < 0)
            {
                index = clips.Count - 1;
            }

            MusicManager.PlayMusic(clips[index]);
        }
    }
}