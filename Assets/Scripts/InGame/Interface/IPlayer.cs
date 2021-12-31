using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace yumehiko
{
    /// <summary>
    /// 操作可能で、かつ死の概念を持つもののうち、いわゆるプレイヤーとみなされるもの。
    /// </summary>
    public interface IPlayer : IContorlable, IDieable
    {

    }
}
