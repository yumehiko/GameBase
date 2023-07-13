using yumehiko.Platformer.Actors.Parts;

namespace yumehiko.Platformer.Entity.Parts
{
    /// <summary>
    /// 接触性。
    /// </summary>
    public interface ITouchable
    {
        System.IObservable<IInteractor> OnTouchEnter { get; }
        System.IObservable<IInteractor> OnTouchExit { get; }
        void TouchEnter(IInteractor toucher);
        void TouchExit(IInteractor toucher);
    }
}
