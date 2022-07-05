using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using yumehiko.Resident;

namespace yumehiko
{
    /// <summary>
    /// ゲーム画面がフォーカスされるまで待機し、ボタンをクリックされたら1番目のシーンをロード。
    /// </summary>
    public class InitScene : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private void Awake()
        {
            _ = startButton.OnClickAsObservable()
                .First()
                .Subscribe(_ => LoadManager.RequireLoadScene(1))
                .AddTo(this);
        }
    }
}