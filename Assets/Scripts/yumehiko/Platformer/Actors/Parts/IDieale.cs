using UniRx;

namespace yumehiko.Platformer.Actors.Parts
{
    /// <summary>
    /// 死亡の概念。
    /// </summary>
    public interface IDieable
    {
        /// <summary>
        /// 死亡したか。
        /// </summary>
        IReadOnlyReactiveProperty<bool> IsDied { get; }

        /// <summary>
        /// 死亡する。
        /// </summary>
        void Die();
    }
}