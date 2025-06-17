namespace BorschtCraft.Food
{
    public class ProductionItemHandler : ItemHandlerBase
    {
        private IConsumable _consumable;

        protected override bool CanHandle(IItem item)
        {
            if (item is IConsumable consumable && consumable is ICantDecorate)
            {
                _consumable = consumable;
                return true;
            }

            _consumable = null;
            return false;
        }

        protected override bool Process(IItem item)
        {
            Logger.LogInfo(this, "Attempting initial production.");

            var isConsumed = _consumable.TryConsume(null, out var consumed);
            if (!isConsumed)
            {
                Logger.LogWarning(this, $"{_consumable.GetType().Name}.TryConsume(null) did not produce an item. This is expected if it's purely a decorator.");
                return false;
            }

            var slotFound = SlotFinderHelper.TryFindSlot(_consumable, out var targetSlot);

            if (!slotFound)
            {
                Logger.LogWarning(this, $"No suitable slot found for {_consumable.GetType().Name}. Cannot produce item.");
                return false;
            }

            Logger.LogInfo(this, $"Produced {consumed.GetType().Name} into a {targetSlot.SlotType} slot.");
            targetSlot.SetItem(consumed);
            return true;
        }
    }
}
