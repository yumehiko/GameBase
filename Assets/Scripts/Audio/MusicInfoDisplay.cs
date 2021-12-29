using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace yumehiko.Audio.Music
{
    /// <summary>
    /// ミュージックの情報を表示する。
    /// </summary>
    public class MusicInfoDisplay : MonoBehaviour
    {
        [SerializeField] private Text titleText;
        [SerializeField] private Text composerText;

        private void Awake()
        {
            _ = MusicManager.OnStartMusic.Subscribe(clip => DisplayInfo(clip)).AddTo(this);
        }

        /// <summary>
        /// 曲名、作曲者の情報を表示。
        /// </summary>
        /// <param name="clip"></param>
        private void DisplayInfo(MusicClip clip)
        {
            if (clip == null)
            {
                titleText.text = string.Empty;
                return;
            }

            titleText.text = clip.Title;
            composerText.text = clip.Composer;
        }
    }
}