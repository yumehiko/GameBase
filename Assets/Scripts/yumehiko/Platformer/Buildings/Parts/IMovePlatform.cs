using UniRx;
using UnityEngine;

namespace yumehiko.Platformer.Buildings.Parts
{
    /// <summary>
    /// 動く足場。
    /// </summary>
    public interface IMovePlatform
    {
        /// <summary>
        /// 乗っているキャラクターに与える速度。
        /// </summary>
        IReadOnlyReactiveProperty<Vector2> VelocityToRider { get; }
    }
}
