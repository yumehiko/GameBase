using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

namespace yumehiko
{
    /// <summary>
    /// プレイヤーがアクションボタンで実行するアクション。
    /// </summary>
    public interface IPlayerAction
    {
        ReadOnlyReactiveProperty<bool> OnAction { get; }
    }
}