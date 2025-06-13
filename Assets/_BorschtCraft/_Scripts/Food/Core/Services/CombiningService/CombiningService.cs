using BorschtCraft.Food.UI;
using Zenject;
using BorschtCraft.Food.Core.Interfaces; // Added

namespace BorschtCraft.Food
{
    public class CombiningService : ICombiningService
    {
        private readonly IItemSlot[] _releasingSlots; // Changed type

        public bool AttemptCombination(IConsumable consumable)
        {
            Logger.LogInfo(this, $"AttemptCombination called for {consumable?.GetType().Name}");

            if (consumable == null)
                return false;

            IItemSlot targetSlotWithItem = FindDecoratableItemSlot(consumable); // Changed type

            if (targetSlotWithItem == null || targetSlotWithItem.GetCurrentItem() == null) // Changed access
            {
                Logger.LogInfo(this, $"No suitable item found in releasing slots to decorate with {consumable.GetType().Name}.");
                return false;
            }

            IConsumed itemToDecorate = targetSlotWithItem.GetCurrentItem(); // Changed access
            Logger.LogInfo(this, $"Found '{itemToDecorate.GetType().Name}' in slot '{targetSlotWithItem.GetGameObject().name}' to decorate with '{consumable.GetType().Name}'."); // Changed access
            IConsumed decoratedItem = consumable.Consume(itemToDecorate);

            if (decoratedItem != null && decoratedItem != itemToDecorate)
            {
                targetSlotWithItem.TrySetItem(decoratedItem);
                Logger.LogInfo(this, $"Successfully decorated '{itemToDecorate.GetType().Name}' with '{consumable.GetType().Name}'. New top layer: '{decoratedItem.GetType().Name}'.");
                return true;
            }
            else
            {
                Logger.LogWarning(this, $"Decorating '{itemToDecorate.GetType().Name}' with '{consumable.GetType().Name}' did not result in a new item or failed.");
                return false;
            }
        }

        private IItemSlot FindDecoratableItemSlot(IConsumable decorator) // Changed return type
        {
            if (_releasingSlots == null || decorator == null) return null;

            Logger.LogInfo(this, $"FindDecoratableItemSlot: Searching for item that '{decorator.GetType().Name}' can decorate.");
            foreach (var slot in _releasingSlots) // slot is IItemSlot
            {
                if (slot != null && slot.GetCurrentItem() != null) // Changed access
                {
                    Logger.LogInfo(this, $"FindDecoratableItemSlot: Checking slot '{slot.GetGameObject().name}' with item '{slot.GetCurrentItem().GetType().Name}'."); // Changed access
                    if (decorator.CanDecorate(slot.GetCurrentItem())) // Changed access
                    {
                        Logger.LogInfo(this, $"FindDecoratableItemSlot: Found suitable slot '{slot.GetGameObject().name}' with item '{slot.GetCurrentItem().GetType().Name}'."); // Changed access
                        return slot;
                    }
                    else
                    {
                        Logger.LogInfo(this, $"FindDecoratableItemSlot: Decorator '{decorator.GetType().Name}' cannot decorate item '{slot.GetCurrentItem().GetType().Name}' in slot '{slot.GetGameObject().name}'."); // Changed access
                    }
                }
            }
            Logger.LogInfo(this, $"FindDecoratableItemSlot: No suitable item found in any releasing slot.");
            return null;
        }

        public CombiningService([Inject(Id = "ReleasingSlots")] IItemSlot[] releasingSlots) // Changed parameter type
        {
            _releasingSlots = releasingSlots;
        }
    }
}
