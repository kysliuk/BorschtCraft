using UniRx;

namespace BorschtCraft.Food
{
    public interface IItemSlot
    {
        IReadOnlyReactiveProperty<IConsumed> CurrentItem { get; }
        SlotType Type { get; }
        bool TrySetItem(IConsumed newItem);
        IConsumed ReleaseItem();
        bool IsEmpty();
    }
}