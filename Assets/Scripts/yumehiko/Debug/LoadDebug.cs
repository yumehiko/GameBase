using UniRx;
using UnityEngine;
using yumehiko.Resident;

namespace yumehiko.Debug
{
    public class LoadDebug : MonoBehaviour
    {
        private void Start()
        {
            LoadManager.OnLoadComplete
                .Subscribe(_ => UnityEngine.Debug.Log("LoadComplete"))
                .AddTo(this);

            LoadManager.OnLoadTransitionEnd
                .Subscribe(_ => UnityEngine.Debug.Log("TransitionEnd"))
                .AddTo(this);
        }
    }
}
