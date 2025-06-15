using UniRx;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class SlotViewModel
    {
        public IReadOnlyReactiveProperty<IConsumed> CurrentItem => _slotModel.Item;

        protected readonly ISlot _slotModel;
        protected readonly SignalBus _signalBus;

        public virtual void SetItem(IConsumed item)
        {
            if (item == null)
            {
                Logger.LogWarning(this, "Attempted to set a null item in the slot.");
                return;
            }

            _slotModel.SetItem(item);
        }

        public virtual void ReleaseItem()
        {
            _slotModel.Release();
            _signalBus.Fire(new ReleaseSlotItemSignal(_slotModel));
        }

        public SlotViewModel(ISlot slotModel, SignalBus signalBus)
        {
            _slotModel = slotModel;
            _signalBus = signalBus;
        }
    }
}
