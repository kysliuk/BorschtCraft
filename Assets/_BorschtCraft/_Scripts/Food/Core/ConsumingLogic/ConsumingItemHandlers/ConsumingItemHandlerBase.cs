namespace BorschtCraft.Food
{
    public abstract class ConsumingItemHandlerBase : ItemHandlerBase
    {
        protected ISlotMatchingStrategy _strategy;
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
            foreach (var slot in SlotHolder.Slots)
            {
                if (_consumable.TryConsume(slot.Item.Value, out var consumed) && _strategy.Matches(slot, consumed))
                {
                    Logger.LogInfo(this, $"Placed {consumed.GetType().Name} into {slot.SlotType} slot.");
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
