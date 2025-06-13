using UniRx;

namespace BorschtCraft.Food
{
    public class ItemSlot : IItemSlot
    {
        public IReadOnlyReactiveProperty<IConsumed> CurrentItem => _currentItem;
        private readonly ReactiveProperty<IConsumed> _currentItem = new ReactiveProperty<IConsumed>(null);

        public SlotType Type { get; }

        public ItemSlot(SlotType type)
        {
            Type = type;
        }

        public bool TrySetItem(IConsumed newItem)
        {
            _currentItem.Value = newItem;
            return true;
        }

        public IConsumed ReleaseItem()
        {
            var releasedItem = _currentItem.Value;
            _currentItem.Value = null;
            return releasedItem;
        }

        public bool IsEmpty()
        {
            return _currentItem.Value == null;
        }

        public IConsumed GetCurrentItem() 
        {
            return _currentItem.Value;
        }
    }
}