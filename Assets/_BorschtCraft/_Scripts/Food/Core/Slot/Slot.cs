using UniRx;
using Zenject;

namespace BorschtCraft.Food
{
    public class Slot : ISlot
    {
        public SlotType Type { get; private set; }
        public ReactiveProperty<IConsumed> Item { get; private set; }

        public void SetItem(IConsumed item)
        {
            if (ValidateItem(item))
                Item.Value = item;
        }

        public void Release()
        {
            ClearCurrentItem();
        }

        private void ClearCurrentItem()
        {
            if (Item.Value != null)
                Item.Value = null;
        }

        private bool ValidateItem(IConsumed item)
        {
            if (item == null)
                return false;

            if (item == Item.Value)
                return false;

            if (Type != SlotType.Cooking && (item is ICookable || item is ICooked))
                return false;

            return true;
        }

        public Slot(SlotType type, IConsumed item, SignalBus signalBus)
        {
            Type = type;
            Item.Value = item;
        }
    }
}
