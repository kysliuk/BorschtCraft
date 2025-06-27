namespace BorschtCraft.Food
{
    public class DrinkingItemHandler : ConsumingItemHandlerBase<DrinkingSlotStrategy>
    {
        protected override bool ProcessSync(IItem item)
        {
            return base.ProcessSync(item);
        }
    }
}
