namespace BorschtCraft.Food
{
    public class CookableItemHandler : ConsumingItemHandlerBase
    {
        protected override ISlotMatchingStrategy SetStrategy()
        {
            return new CookingSlotStrategy();
        }

        public CookableItemHandler(IItemHandler itemHandler)
        {
            SetNext(itemHandler);
        }
    }
}
