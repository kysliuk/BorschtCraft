namespace BorschtCraft.Food
{
    public abstract class ConsumingItemHandlerBase : ItemHandlerBase
    {
        protected ISlotMatchingStrategy _strategy => SetStrategy();
        private IConsumable _consumable;

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
                    Logger.LogInfo(this, $"About to place {consumed.GetType().Name} into {slot.SlotType} slot.");
                    slot.SetItem(consumed);
                    return true;
                }
            }

            Logger.LogWarning(this, $"No matching slot for {_consumable.GetType().Name}.");
            return false;
        }

        protected abstract ISlotMatchingStrategy SetStrategy();
    }
}
