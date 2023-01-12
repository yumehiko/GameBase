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
        [SerializeField] private Text musicTitleText;
        private int index = 0;

        private void Awake()
        {
            nextButton.OnClickAsObservable().Subscribe(_ => PlayNext());
            prevButton.OnClickAsObservable().Subscribe(_ => PlayPrev());
        }

        private void PlayNext()
        {
            index++;
            if (index >= clips.Count)
            {
                index = 0;
            }
            var clip = clips[index];
            musicTitleText.text = clip.Title;
            MusicManager.PlayMusic(clip);
        }

        private void PlayPrev()
        {
            index--;
            if (index < 0)
            {
                index = clips.Count - 1;
            }

            var clip = clips[index];
            musicTitleText.text = clip.Title;
            MusicManager.PlayMusic(clip);
        }
    }
}