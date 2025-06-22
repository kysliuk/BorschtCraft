namespace BorschtCraft.Food
{
    public class DrinkingItemHandler : ConsumingItemHandlerBase<DrinkingSlotStrategy>
    {
        protected override bool Process(IItem item)
        {
            Logger.LogWarning(this, $"Drinking item handler is not implemented yet. Item of type {item.GetType().Name} was consumed in drinking slot.");
            return base.Process(item);
        }
    }
}
