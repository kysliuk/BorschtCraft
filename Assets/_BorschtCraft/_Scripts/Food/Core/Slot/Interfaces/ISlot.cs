using UniRx;

namespace BorschtCraft.Food
{
    public interface ISlot
    {
        SlotType SlotType { get; }
        IReadOnlyReactiveProperty<IConsumed> Item { get;}
        bool TrySetItem(IConsumed item);
    }
}
