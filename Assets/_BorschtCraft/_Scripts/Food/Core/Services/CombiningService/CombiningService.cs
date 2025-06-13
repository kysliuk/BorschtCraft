using BorschtCraft.Food.UI;
using Zenject;

namespace BorschtCraft.Food
{
    public class CombiningService : ICombiningService
    {
        private readonly ItemSlotController[] _releasingSlots;

        public bool AttemptCombination(IConsumable consumable)
        {
            Logger.LogInfo(this, $"AttemptCombination called for {consumable?.GetType().Name}");

            if (consumable == null)
                return false;

            ItemSlotController targetSlotWithItem = FindDecoratableItemSlot(consumable);

            if (targetSlotWithItem == null || targetSlotWithItem.CurrentItemInSlot == null)
            {
                Logger.LogInfo(this, $"No suitable item found in releasing slots to decorate with {consumable.GetType().Name}.");
                return false; 
            }

            IConsumed itemToDecorate = targetSlotWithItem.CurrentItemInSlot;
            Logger.LogInfo(this, $"Found '{itemToDecorate.GetType().Name}' in slot '{targetSlotWithItem.gameObject.name}' to decorate with '{consumable.GetType().Name}'.");
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

        private ItemSlotController FindDecoratableItemSlot(IConsumable decorator)
        {
            if (_releasingSlots == null || decorator == null) return null;

            Logger.LogInfo(this, $"FindDecoratableItemSlot: Searching for item that '{decorator.GetType().Name}' can decorate.");
            foreach (var slot in _releasingSlots)
            {
                if (slot != null && slot.CurrentItemInSlot != null)
                {
                    Logger.LogInfo(this, $"FindDecoratableItemSlot: Checking slot '{slot.gameObject.name}' with item '{slot.CurrentItemInSlot.GetType().Name}'.");
                    if (decorator.CanDecorate(slot.CurrentItemInSlot))
                    {
                        Logger.LogInfo(this, $"FindDecoratableItemSlot: Found suitable slot '{slot.gameObject.name}' with item '{slot.CurrentItemInSlot.GetType().Name}'.");
                        return slot; 
                    }
                    else
                    {
                        Logger.LogInfo(this, $"FindDecoratableItemSlot: Decorator '{decorator.GetType().Name}' cannot decorate item '{slot.CurrentItemInSlot.GetType().Name}' in slot '{slot.gameObject.name}'.");
                    }
                }
            }
            Logger.LogInfo(this, $"FindDecoratableItemSlot: No suitable item found in any releasing slot.");
            return null;
        }

        public CombiningService([Inject(Id = "ReleasingSlots")] ItemSlotController[] releasingSlots)
        {
            _releasingSlots = releasingSlots;
        }
    }
}
