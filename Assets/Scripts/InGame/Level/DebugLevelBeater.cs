using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using yumehiko.Resident.Control;

namespace yumehiko
{
    public class DebugLevelBeater : MonoBehaviour
    {
        [SerializeField] private Level level;

        private void Awake()
        {
            _ = ReactiveInput.OnDebug.Where(isOn => isOn).Subscribe(_ => level.BeatLevel(1)).AddTo(this);
        }
    }
}
