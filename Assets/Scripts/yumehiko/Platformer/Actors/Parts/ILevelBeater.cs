namespace yumehiko.Platformer.Actors.Parts
{
    /// <summary>
    /// レベルをクリアできるもの。
    /// </summary>
    public interface ILevelBeater
    {
        bool CanBeat { get; }
        void OnBeatLevel();
    }
}