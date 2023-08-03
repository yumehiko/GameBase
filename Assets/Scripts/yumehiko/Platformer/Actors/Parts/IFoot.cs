using UniRx;

namespace yumehiko.Platformer.Actors.Parts
{
    /// <summary>
    /// 接地判定。
    /// </summary>
    public interface IFoot
    {
        IReadOnlyReactiveProperty<bool> IsGrounded { get; }
        float FallSpeedOnLastFrame { get; }

        /// <summary>
        /// 降りれるタイプの足場を降りる。
        /// </summary>
        void DownPlatform();
    }
}