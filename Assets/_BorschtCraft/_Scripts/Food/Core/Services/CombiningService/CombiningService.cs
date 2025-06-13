using Zenject;

namespace BorschtCraft.Food
{
    public class CombiningService : ICombiningService
    {
        private readonly IItemSlot[] _releasingSlots;

        public CombiningService([Inject(Id = "ReleasingSlots")] IItemSlot[] releasingSlots)
        {
            _releasingSlots = releasingSlots;
        }

        public bool AttemptCombination(IConsumable consumable)
        {
            if (consumable == null) return false;

            IItemSlot targetSlot = FindDecoratableItemSlot(consumable);
            if (targetSlot == null)
            {
                Logger.LogInfo(this, $"No suitable item found to decorate with {consumable.GetType().Name}.");
                return false;
            }

            IConsumed itemToDecorate = targetSlot.CurrentItem.Value;
            IConsumed decoratedItem = consumable.Consume(itemToDecorate);

            if (decoratedItem != itemToDecorate)
            {
                targetSlot.TrySetItem(decoratedItem);
                Logger.LogInfo(this, $"Successfully decorated '{itemToDecorate.GetType().Name}'.");
                return true;
            }

            Logger.LogWarning(this, $"Decorating '{itemToDecorate.GetType().Name}' failed.");
            return false;
        }

        private IItemSlot FindDecoratableItemSlot(IConsumable decorator)
        {
            foreach (var slot in _releasingSlots)
            {
                if (!slot.IsEmpty() && decorator.CanDecorate(slot.CurrentItem.Value))
                {
                    return slot;
                }
            }
            return null;
        }
    }
}