using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Resident
{
    public class GameManager : MonoBehaviour
    {
        public static bool IsNormalSystem { get; private set; } = false;

        private void Awake()
        {
            IsNormalSystem = true;
            DontDestroyOnLoad(gameObject);
        }
    }
}
