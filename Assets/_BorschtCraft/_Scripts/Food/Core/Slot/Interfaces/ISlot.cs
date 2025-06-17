using UniRx;

namespace BorschtCraft.Food
{
    public interface ISlot
    {
        SlotType SlotType { get; }
        IReadOnlyReactiveProperty<IConsumed> Item { get;}
        void SetItem(IConsumed item);
        void Release();
    }
}
