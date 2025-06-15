using UniRx;

namespace BorschtCraft.Food
{
    public interface ISlot : IReleasable
    {
        SlotType Type { get; }
        ReactiveProperty<IConsumed> Item { get;}
        void SetItem(IConsumed item);
    }
}
