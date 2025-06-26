using System.Threading.Tasks;

namespace BorschtCraft.Food
{
    public class CookingSlotReleasingHandler : SlotReleasingHandlerBase<ReleasingCookingSlotStrategy>
    {
        protected override Task<bool> ProcessItemReleasing(ISlot slot)
        {
            Logger.LogInfo(this, $"Trying to place item of type {_consumed.GetType().Name} in combining slot: {_slotRegistry.Slots.Length}.");

            var emptySlot = _slotRegistry.GetEmptySlot(SlotType.Combining);
            if (emptySlot == null)
            {
                Logger.LogInfo(this, "No empty combining slot found.");
                return Task.FromResult(false);
            }

            var cooked = _consumed as ICooked;
            if (cooked == null)
            {
                Logger.LogWarning(this, "Consumed item is not ICooked.");
                return Task.FromResult(false);
            }

            var canBePlaced = cooked.CanPlaceOnTop(emptySlot.Item.Value, out var itemToPlace);

            if (!canBePlaced)
            {
                Logger.LogInfo(this, $"Item of type {itemToPlace?.GetType().Name} cannot be placed on top of the item in combining slot: {emptySlot.Item.Value?.GetType().Name}.");
                return Task.FromResult(false);
            }

            var set = emptySlot.TrySetItem(itemToPlace);
            Logger.LogInfo(this, $"Item of type {itemToPlace.GetType().Name} placed in combining slot: {set}.");

            return Task.FromResult(set);
        }
    }
}
