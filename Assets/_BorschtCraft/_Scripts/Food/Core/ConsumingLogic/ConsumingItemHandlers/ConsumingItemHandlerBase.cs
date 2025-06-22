using System.Linq;

namespace BorschtCraft.Food
{
    public abstract class ConsumingItemHandlerBase<T> : StrategizedItemHandler<T>, IConsumingItemHandler where T : ISlotMatchingStrategy
    {
        protected IConsumable _consumable;
        protected ISlot _selectedSlot;

        protected override bool CanHandle(IItem item)
        {
            if (item is not IConsumable c)
                return false;

            _consumable = c;
            return true;
        }

        protected override bool Process(IItem item)
        {
            var slots = _slotRegistry.Slots
                .Where(s => s.SlotType == _strategy.SlotType);

            foreach (var slot in slots)
            {
                if (_consumable.TryConsume(slot.Item.Value, out var consumed) && _strategy.Matches(slot, consumed))
                {
                    _selectedSlot = slot;
                    Logger.LogInfo(this, $"Item of type {_consumable.GetType().Name} matches slot of type {slot.SlotType}.");
                    return slot.TrySetItem(consumed);
                }
            }

            Logger.LogInfo(this, $"No matching slot for {_consumable.GetType().Name}.");
            return false;
        }

        protected override void OnInitialize()
        {
            _signalBus.Subscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        protected override void OnDispose()
        {
            _signalBus.Unsubscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        private void OnConsumableInteractionRequested(ConsumableInteractionRequestSignal signal)
        {
            Logger.LogInfo(this, $"Received request for {signal.ConsumableSource.GetType().Name}. Starting handler chain.");
            Handle(signal.ConsumableSource);
        }
    }
}
