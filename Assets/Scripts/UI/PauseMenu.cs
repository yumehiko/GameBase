using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using yumehiko.Resident;

namespace yumehiko.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;

        public ReadOnlyReactiveProperty<bool> IsPause { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            PauseManager.IsPause
                .Subscribe(isOn => SwitchPanel(isOn))
                .AddTo(this);

            IsPause = PauseManager.IsPause;

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