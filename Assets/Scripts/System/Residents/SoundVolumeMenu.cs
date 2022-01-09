using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UniRx;

namespace yumehiko.UI
{
    /// <summary>
    /// 音量調節のメニュー。
    /// </summary>
    public class SoundVolumeMenu : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private PauseMenu pauseMenu;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundSlider;

        private readonly string aKeyMasterVolume = "MasterVolume";
        private readonly string aKeyMusicVolume = "MusicVolume";
        private readonly string aKeySoundVolume = "SoundVolume";


        private void Start()
        {
            //memo: ゲーム起動時にAwakeで呼び出すと、AudioMixerが初期値を呼び出すタイミングとかぶって問題になる（editor上のみ）
            SubscribePauseMenu();
            SubscribeSliders();
            LoadPrefs();
        }


        /// <summary>
        /// PlayerPrefのデータをロードし、スライダーに反映。
        /// </summary>
        private void LoadPrefs()
        {
            masterSlider.value = PlayerPrefs.GetFloat(aKeyMasterVolume, 0.8f);
            musicSlider.value = PlayerPrefs.GetFloat(aKeyMusicVolume, 0.6f);
            soundSlider.value = PlayerPrefs.GetFloat(aKeySoundVolume, 0.6f);
        }

        /// <summary>
        /// 現在値をPlayerPrefに保存。
        /// </summary>
        private void SaveToPrefs()
        {
            PlayerPrefs.SetFloat(aKeyMasterVolume, masterSlider.value);
            PlayerPrefs.SetFloat(aKeyMusicVolume, musicSlider.value);
            PlayerPrefs.SetFloat(aKeySoundVolume, soundSlider.value);
        }

        /// <summary>
        /// ポーズメニューイベントを購読。
        /// </summary>
        private void SubscribePauseMenu()
        {
            //ポーズを開いたとき、ユーザーデータをロードする。
            _ = pauseMenu.IsPause
                .Where(isOn => isOn)
                .Subscribe(_ => LoadPrefs())
                .AddTo(this);

            //ポーズを閉じたとき、ユーザーデータをセーブする。
            _ = pauseMenu.IsPause
                .Where(isOn => !isOn)
                .Skip(1)
                .Subscribe(_ => SaveToPrefs())
                .AddTo(this);
        }

        /// <summary>
        /// 各音量スライダーを購読。
        /// </summary>
        private void SubscribeSliders()
        {
            masterSlider.OnValueChangedAsObservable().Subscribe(value =>
            {
                value = ValueToDecibel(value);
                audioMixer.SetFloat(aKeyMasterVolume, value);
            }).AddTo(this);

            musicSlider.OnValueChangedAsObservable().Subscribe(value =>
            {
                value = ValueToDecibel(value);
                audioMixer.SetFloat(aKeyMusicVolume, value);
            }).AddTo(this);

            soundSlider.OnValueChangedAsObservable().Subscribe(value =>
            {
                value = ValueToDecibel(value);
                audioMixer.SetFloat(aKeySoundVolume, value);
            }).AddTo(this);
        }

        /// <summary>
        /// 正規値をデシベルに変換。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float ValueToDecibel(float value)
        {
            value = Mathf.Clamp(value, 0.0001f, 1.0f);
            return 20.0f * Mathf.Log10(value);
        }
    }
}