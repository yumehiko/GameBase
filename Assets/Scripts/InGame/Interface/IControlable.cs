using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko
{
    /// <summary>
    /// コントロール性。
    /// </summary>
    public interface IContorlable
    {
        /// <summary>
        /// コントロール可能か。
        /// </summary>
        bool CanControl { get; }

        /// <summary>
        /// コントロール可能かどうかを切り替える。
        /// </summary>
        /// <param name="isOn"></param>
        void SetCanControl(bool isOn);
    }
}
