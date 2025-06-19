namespace BorschtCraft.Food
{
    public abstract class ConsumingItemHandlerBase<T> : StrategizedItemHandler<T>, IConsumingItemHandler where T : ISlotMatchingStrategy
    {
        protected IConsumable _consumable;
        protected ISlot _selectedSlot;

        protected override bool CanHandle(IItem item)
        {
            Logger.LogWarning(this, $"Strategy type {typeof(T).Name}. SlotType is {_strategy.SlotType}");

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
                Logger.LogInfo(this, $"Checking slot of type {slot.SlotType} with item {slot.Item.Value?.GetType().Name ?? "null"} for consumable {_consumable.GetType().Name}.");

                if (_consumable.TryConsume(slot.Item.Value, out var consumed) && _strategy.Matches(slot, consumed))
                {
                    Logger.LogInfo(this, $"About to place {consumed.GetType().Name} into {slot.SlotType} slot.");
                    _selectedSlot = slot;
                    return slot.TrySetItem(consumed);
                }
            }

            Logger.LogWarning(this, $"No matching slot for {_consumable.GetType().Name}.");
            return false;
        }

    }
}
