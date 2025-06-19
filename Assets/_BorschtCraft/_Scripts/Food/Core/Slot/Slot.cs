using UniRx;
using Zenject;

namespace BorschtCraft.Food
{
    public class Slot : ISlot
    {
        public SlotType SlotType { get; private set; }
        public IReadOnlyReactiveProperty<IConsumed> Item => _item;

        private ReactiveProperty<IConsumed> _item;

        public void SetItem(IConsumed item)
        {
            if (ValidateItem(item))
            {
                _item.Value = item;
                Logger.LogInfo(this, $"Item of type {item.GetType().Name} was set in slot of type {SlotType}.");
            }
            else
                throw new System.ArgumentException("Invalid item for the slot.");
        }

        public void Release()
        {
            ClearCurrentItem();
        }

        private void ClearCurrentItem()
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

            if (SlotType != SlotType.Cooking && (item is ICookable || item is ICooked))
                return false;

            return true;
        }

        public Slot(SlotType type, IConsumed item)
        {
            SlotType = type;
            _item = new ReactiveProperty<IConsumed>(item);
        }
    }
}
