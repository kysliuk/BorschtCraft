namespace BorschtCraft.Food
{
    public class CombiningItemHandler : ConsumingItemHandlerBase<ConsumingInCombiningSlotStrategy>
    {
        protected override bool CanHandle(IItem item)
        {
            if (_strategy.SlotType != SlotType.Combining)
                return false;

            return base.CanHandle(item);
        }
    }
}
