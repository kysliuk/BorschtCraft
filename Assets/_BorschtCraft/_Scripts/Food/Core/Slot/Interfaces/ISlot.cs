using UniRx;

namespace BorschtCraft.Food
{
    public interface ISlot : IReleasable
    {
        SlotType SlotType { get; }
        ReactiveProperty<IConsumed> Item { get;}
        void SetItem(IConsumed item);
    }
}
