using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using yumehiko.Resident;
using yumehiko.Resident.Control;

namespace yumehiko.Resident
{
    /// <summary>
    /// ポーズメニュー。ポーズキーで停止し、メニューを表示する。
    /// ゲーム中常に存在する。
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        public IReadOnlyReactiveProperty<bool> IsPause { get; } = PauseManager.IsPause;


        [SerializeField] private GameObject pausePanel;


        private void Start()
        {
            //親が存在するなら、ロード時に破壊するかは親が決める。
            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }

            PauseManager.IsPause
                .Subscribe(isTrue => SwitchPanel(isTrue))
                .AddTo(this);

            ReactiveInput.OnPause
                .Where(isTrue => isTrue)
                .Subscribe(_ => PauseManager.SwitchPause())
                .AddTo(this);
        }

        private void SwitchPanel(bool isOn)
        {
            pausePanel.SetActive(isOn);
        }
    }
}