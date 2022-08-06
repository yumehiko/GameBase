using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.Resident;
using UniRx;

namespace yumehiko
{
    public class LoadDebug : MonoBehaviour
    {
        private void Start()
        {
            LoadManager.OnLoadComplete
                .Subscribe(_ => Debug.Log("LoadComplete"))
                .AddTo(this);

            LoadManager.OnLoadTransitionEnd
                .Subscribe(_ => Debug.Log("TransitionEnd"))
                .AddTo(this);
        }
    }
}
