namespace BorschtCraft.Food
{
    public class CookingItemHandler : ConsumingItemHandlerBase<ConsumingInCookingSlotStrategy>
    {
        protected override bool Process(IItem item)
        {
            var hasProcessed = base.Process(item);
            if (hasProcessed)
            {
                _signalBus.Fire(new CookItemInSlotSignal(_selectedSlot));
                Logger.LogInfo(this, $"Fired {nameof(CookItemInSlotSignal)} for item: {item.GetType().Name} in slot: {_selectedSlot.GetHashCode()}");
            }
            return hasProcessed;
        }
    }
}
