using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko
{
    /// <summary>
    /// 操作可能なもののうち、いわゆるプレイヤーとみなされるもの。
    /// </summary>
    public interface IPlayer
    {
        IContorlable contorlable { get; }
    }
}
