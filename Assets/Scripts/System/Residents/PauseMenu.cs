using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using yumehiko.Resident;
using yumehiko.Resident.Control;

namespace yumehiko.UI
{
    /// <summary>
    /// ポーズメニュー。ポーズキーで停止し、メニューを表示する。
    /// ゲーム中常に存在する。
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;

        public ReadOnlyReactiveProperty<bool> IsPause { get; } = PauseManager.IsPause;

        private void Start()
        {
            //親が存在するなら、ロード時に破壊するかは親が決める。
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }

            PauseManager.IsPause
                .Subscribe(isOn => SwitchPanel(isOn))
                .AddTo(this);

            ReactiveInput.OnPause
                .Where(isOn => isOn)
                .Subscribe(isOn => PauseManager.SwitchPause())
                .AddTo(this);
        }

        private void SwitchPanel(bool isOn)
        {
            pausePanel.SetActive(isOn);
        }
    }
}