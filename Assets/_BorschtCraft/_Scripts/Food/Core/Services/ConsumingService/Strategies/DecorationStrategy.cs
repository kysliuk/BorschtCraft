namespace BorschtCraft.Food.Core.Services.ConsumingService.Strategies
{
    public class DecorationStrategy : IConsumptionStrategy
    {
        private readonly ICombiningService _combiningService;

        public DecorationStrategy(ICombiningService combiningService)
        {
            _combiningService = combiningService;
        }

        public bool TryExecute(IConsumable consumableSource,
                                 IItemSlot[] availableCookingSlots,
                                 IItemSlot[] availableReleasingSlots, // This parameter is not directly used here as CombiningService has its own slots
                                 out IConsumed finalItem, // Changed from resultingItem to finalItem to match IConsumptionStrategy
                                 out IItemSlot finalSlot)   // Changed from decoratedSlot to finalSlot
        {
            finalItem = null;
            finalSlot = null;

            if (consumableSource is ICantDecorate)
            {
                Logger.LogInfo(this, $"{consumableSource.GetType().Name} is marked with ICantDecorate. Skipping decoration attempt via strategy.");
                return false;
            }

            // availableReleasingSlots from parameters are not used here because CombiningService is configured with its own set of slots upon injection.
            // If the design intended DecorationStrategy to tell CombiningService which slots to use, CombiningService's interface would need to change further.
            // For now, DecorationStrategy relies on CombiningService's pre-configured slots.
            if (_combiningService.AttemptCombination(consumableSource, out finalItem, out finalSlot))
            {
               Logger.LogInfo(this, $"DecorationStrategy: CombiningService successfully handled {consumableSource.GetType().Name}. Resulting item: {finalItem?.GetType().Name}, Slot: {finalSlot?.GetGameObject().name}.");
               return true;
            }

            Logger.LogInfo(this, $"DecorationStrategy: CombiningService did not handle {consumableSource.GetType().Name}, or no suitable item to decorate.");
            return false;
        }
    }
}
