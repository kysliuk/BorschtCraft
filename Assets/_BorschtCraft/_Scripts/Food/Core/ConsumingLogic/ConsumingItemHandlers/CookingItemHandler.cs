using Zenject;

namespace BorschtCraft.Food
{
    public class CookingItemHandler : ConsumingItemHandlerBase<ConsumingInCookingSlotStrategy>
    {
        protected readonly SignalBus _signalbus;

        protected override bool CanHandle(IItem item)
        {
            Logger.LogWarning(this, $"SlotType equals Cooking {_strategy.SlotType == SlotType.Cooking}");
            if (_strategy.SlotType != SlotType.Cooking)
                return false;

            return base.CanHandle(item);
        }

        protected override bool Process(IItem item)
        {
            var hasProcessed = base.Process(item);
            if (hasProcessed)
            {
                _signalbus.Fire(new CookItemInSlotSignal(_selectedSlot));
                Logger.LogInfo(this, $"Fired {nameof(CookItemInSlotSignal)} for item: {item.GetType().Name} in slot: {_selectedSlot.GetHashCode()}");
            }
            return hasProcessed;
        }

        public CookingItemHandler(SignalBus signalBus)
        {
            _signalbus = signalBus;
        }
    }
}
