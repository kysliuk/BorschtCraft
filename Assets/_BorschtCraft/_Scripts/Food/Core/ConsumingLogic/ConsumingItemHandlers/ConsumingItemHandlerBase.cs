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
            Logger.LogInfo(this, $"Processing {item.GetType().Name} for consumption. Number of slots {_slots.Length}.");
            foreach (var slot in _slots)
            {
                if (_consumable.TryConsume(slot.Item.Value, out var consumed) && _strategy.Matches(slot, consumed))
                {
                    _selectedSlot = slot;
                    return slot.TrySetItem(consumed);
                }
            }

            Logger.LogInfo(this, $"No matching slot for {_consumable.GetType().Name}.");
            return false;
        }

    }
}
