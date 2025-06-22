using System;
using System.Linq;

namespace BorschtCraft.Food
{
    public abstract class SlotReleasingHandlerBase<T> : StrategizedItemHandler<T>, ISlotReleasingHandler where T : ISlotMatchingStrategy
    {
        protected IConsumed _consumed;

        protected override bool CanHandle(IItem item)
        {
            if (item is not IConsumed c)
                return false;

            _consumed = c;
            return true;
        }

        protected override bool Process(IItem item)
        {
            var slot = _slotRegistry.Slots.FirstOrDefault(s => _strategy.Matches(s, item) && s.Item.Value == item);
            if (slot != null)
            {
                Logger.LogInfo(this, $"Item of type {item.GetType().Name} matches slot of type {slot.SlotType}.");

                var released = ProcessItemReleasing(slot);
                if (released)
                    slot.ClearCurrentItem();

                return released;
            }

            Logger.LogInfo(this, $"No matching slot for {item.GetType().Name}.");
            return false;
        }

        protected override void OnInitialize()
        {
            _signalBus.Subscribe<ReleaseSlotItemSignal>(OnReleaseSlotItemSignal);
        }

        protected override void OnDispose()
        {
            _signalBus.Unsubscribe<ReleaseSlotItemSignal>(OnReleaseSlotItemSignal);
        }

        protected abstract bool ProcessItemReleasing(ISlot slot);

        private void OnReleaseSlotItemSignal(ReleaseSlotItemSignal signal)
        {
            Logger.LogInfo(this, $"Received request to release item from slot of type {signal.Slot.SlotType}.");

            if (signal.Slot.SlotType == _strategy.SlotType)
                Handle(signal.Slot.Item.Value);
        }
    }
}
