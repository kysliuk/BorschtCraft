namespace BorschtCraft.Food
{
    public class CookingSlotReleasingHandler : SlotReleasingHandlerBase<ReleasingCookingSlotStrategy>
    {
        protected override bool ProcessItemReleasing(ISlot slot)
        {
            Logger.LogInfo(this, $"Trying to place item of type {_consumed.GetType().Name} in combining slot: {_slotRegistry.Slots.Length}.");

            var emptySlot = _slotRegistry.GetEmptySlot(SlotType.Combining);
            if(emptySlot == null)
            {
                Logger.LogInfo(this, "No empty combining slot found.");
                return false;
            }
            
            var canBePlaced = (_consumed as ICooked).CanPlaceOnTop(emptySlot.Item.Value, out var itemToPlace);

            if (!canBePlaced)
            {
                Logger.LogInfo(this, $"Item of type {itemToPlace.GetType().Name} cannot be placed on top of the item in combining slot: {emptySlot.Item.Value.GetType().Name}.");
                return false;
            }
            var setted = emptySlot.TrySetItem(itemToPlace);

            Logger.LogInfo(this, $"Item of type {itemToPlace.GetType().Name} placed in combining slot: {setted}.");
            return setted;
        }
    }
}