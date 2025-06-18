namespace BorschtCraft.Food
{
    public class CombiningItemHandler : ConsumingItemHandlerBase
    {
        protected override ISlotMatchingStrategy SetStrategy()
        {
            return new CombiningSlotStrategy();

        }
    }
}
