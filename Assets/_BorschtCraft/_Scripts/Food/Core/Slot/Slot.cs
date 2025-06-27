using UniRx;
using Zenject;

namespace BorschtCraft.Food
{
    public class Slot : ISlot
    {
        public SlotType SlotType { get; private set; }
        public IReadOnlyReactiveProperty<IConsumed> Item => _item;

        private ReactiveProperty<IConsumed> _item;

        public bool TrySetItem(IConsumed item)
        {
            var canSet = ValidateItem(item);
            if (canSet)
                _item.Value = item;

            return canSet;
        }

        public void ClearCurrentItem()
        {
            if (_item.Value != null)
                _item.Value = null;
            else
                Logger.LogWarning(this, $"Attempted to clear an already empty slot of type {SlotType}.");
        }

        private bool ValidateItem(IConsumed item)
        {
            if (item == null)
                return false;

            if (item == _item.Value)
                return false;

            if (SlotType != SlotType.Cooking && item is ICookable)
            {
                Logger.LogWarning(this, $"Item of type {item.GetType().Name} cannot be set in slot of type {SlotType} because it is not a cooking slot.");
                return false;
            }

            return true;
        }

        public Slot(SlotType type, IConsumed item)
        {
            SlotType = type;
            _item = new ReactiveProperty<IConsumed>(item);
        }
    }
}
