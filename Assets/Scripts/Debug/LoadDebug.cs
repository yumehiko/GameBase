using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.Resident;
using UniRx;

namespace yumehiko
{
    public class LoadDebug : MonoBehaviour
    {
        private void Awake()
        {
            LoadManager.OnLoadComplete
                .Subscribe(_ => Debug.Log("LoadComplete"))
                .AddTo(this);
        }
    }
}
