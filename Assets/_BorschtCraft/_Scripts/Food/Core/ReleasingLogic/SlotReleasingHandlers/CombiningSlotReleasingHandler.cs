using Zenject;

namespace BorschtCraft.Food
{
    public class CombiningSlotReleasingHandler : SlotReleasingHandlerBase<ReleasingCombiningSlotStrategy>
    {
        protected override bool CanHandle(IItem item)
        {
            if (_strategy.SlotType != SlotType.Combining)
                return false;

            return base.CanHandle(item);
        }

        protected override bool ProcessItemReleasing(ISlot item)
        {
            Logger.LogWarning(this, $"Combining slot releasing handler is not implemented yet. Item of type {item.GetType().Name} was released to customer from combining slot.");
            return true;
        }
    }
}
