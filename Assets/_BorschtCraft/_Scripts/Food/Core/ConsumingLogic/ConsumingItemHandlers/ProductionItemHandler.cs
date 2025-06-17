using NUnit.Framework;

namespace BorschtCraft.Food
{
    public class ProductionItemHandler : ItemHandlerBase
    {
        protected override bool CanHandle(IItem item)
        {
            return true;
        }

        protected override bool Process(IItem item)
        {
            Logger.LogInfo(this, "Attempting initial production.");
            if (item is not IConsumable consumable)
                return false;

            var isConsumed = consumable.TryConsume(null, out var consumed);
            if (!isConsumed)
            {
                Logger.LogWarning(this, $"{consumable.GetType().Name}.TryConsume(null) did not produce an item. This is expected if it's purely a decorator.");
                return false;
            }

            var slotFound = SlotFinderHelper.TryFindSlot(consumable, out var targetSlot);

            if (!slotFound)
            {
                Logger.LogWarning(this, $"No suitable slot found for {consumable.GetType().Name}. Cannot produce item.");
                return false;
            }

            Logger.LogInfo(this, $"Produced {consumed.GetType().Name} into a {targetSlot.SlotType} slot.");
            targetSlot.SetItem(consumed);
            return true;
        }
    }
}
