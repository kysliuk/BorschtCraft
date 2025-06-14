namespace BorschtCraft.Food.Core.Services.ConsumingService.Strategies
{
    public interface IConsumptionStrategy
    {
        bool TryExecute(IConsumable consumableSource,
                        IItemSlot[] availableCookingSlots,
                        IItemSlot[] availableReleasingSlots,
                        out IConsumed finalItem,
                        out IItemSlot finalSlot);
    }
}
