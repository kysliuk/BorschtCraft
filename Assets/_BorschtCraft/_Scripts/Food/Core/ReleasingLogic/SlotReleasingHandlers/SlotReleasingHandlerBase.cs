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
            Logger.LogInfo(this, $"Processing item of type {item.GetType().Name} for releasing in slots {_slots.Length}.");
            foreach (var slot in _slots)
            {
                Logger.LogInfo(this, $"Checking if item of type {item.GetType().Name} matches slot of type {slot.SlotType}.");
                if (_strategy.Matches(slot, item))
                {
                    Logger.LogInfo(this, $"Item of type {item.GetType().Name} matches slot of type {slot.SlotType}.");
                    return ProcessItemReleasing(slot);
                }
            }
            return false;
        }

        protected abstract bool ProcessItemReleasing(ISlot slot);
    }
}
