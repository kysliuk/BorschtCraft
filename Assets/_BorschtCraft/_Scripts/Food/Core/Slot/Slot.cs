using UniRx;

namespace BorschtCraft.Food
{
    public class Slot : ISlot
    {
        public SlotType SlotType => _slotConfig.SlotType;

        public IReadOnlyReactiveProperty<IConsumed> Item => _item;

        private readonly SlotConfig _slotConfig;

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
            if (SlotType == SlotType.Cooking && item is ICookable)
                (item as ICookable).CookingTime = _slotConfig.CookingTime;

            return true;
        }

        public Slot(SlotConfig slotConfig, IConsumed item)
        {
            _slotConfig = slotConfig;
            _item = new ReactiveProperty<IConsumed>(item);
        }
    }
}
