namespace BorschtCraft.Food
{
    public class DrinkingSlotReleasingHandler : SlotReleasingHandlerBase<DrinkingSlotStrategy>
    {
        protected override bool ProcessItemReleasing(ISlot slot)
        {
            Logger.LogWarning(this, $"Drinking slot releasing handler is not implemented yet. Item of type {slot.Item.Value.GetType().Name} was released to customer from drinking slot.");
            return true;
        }
    }
}
