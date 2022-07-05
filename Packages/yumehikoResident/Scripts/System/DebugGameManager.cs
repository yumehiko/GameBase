using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko.Resident
{
    public class DebugGameManager : MonoBehaviour
    {
        private void Awake()
        {
            if(GameManager.IsNormalSystem)
            {
                Destroy(gameObject);
            }
        }
    }
}
