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

        public bool AttemptCombination(IConsumable decorator, out IConsumed resultingItem, out IItemSlot decoratedSlot)
        {
            resultingItem = null;
            decoratedSlot = null;

            if (decorator == null)
            {
                Logger.LogWarning(this, "AttemptCombination called with a null decorator.");
                return false;
            }

            Logger.LogInfo(this, $"AttemptCombination called for {decorator.GetType().Name}.");

            if (_releasingSlots == null)
            {
                Logger.LogWarning(this, "Releasing slots array is null.");
                return false;
            }

            foreach (var slot in _releasingSlots)
            {
                if (slot != null && !slot.IsEmpty())
                {
                    IConsumed itemToDecorate = slot.GetCurrentItem();
                    Logger.LogInfo(this, $"Checking slot '{slot.GetGameObject().name}' with item '{itemToDecorate.GetType().Name}' to decorate with '{decorator.GetType().Name}'.");

                    if (decorator.CanDecorate(itemToDecorate))
                    {
                        Logger.LogInfo(this, $"Decorator '{decorator.GetType().Name}' can decorate item '{itemToDecorate.GetType().Name}'. Performing Consume.");
                        IConsumed combinedItem = decorator.Consume(itemToDecorate);

                        if (combinedItem != null && combinedItem != itemToDecorate) // Ensure a new item was created
                        {
                            slot.TrySetItem(combinedItem);
                            resultingItem = combinedItem;
                            decoratedSlot = slot;
                            Logger.LogInfo(this, $"Successfully decorated '{itemToDecorate.GetType().Name}' with '{decorator.GetType().Name}'. New top layer: '{resultingItem.GetType().Name}' in slot {decoratedSlot.GetGameObject().name}.");
                            return true;
                        }
                        else
                        {
                            Logger.LogWarning(this, $"Decorating '{itemToDecorate.GetType().Name}' with '{decorator.GetType().Name}' did not result in a new distinct item or failed.");
                        }
                    }
                    else
                    {
                        Logger.LogInfo(this, $"Decorator '{decorator.GetType().Name}' cannot decorate item '{itemToDecorate.GetType().Name}' in slot '{slot.GetGameObject().name}'.");
                    }
                }
            }

            Logger.LogInfo(this, $"No suitable item found in releasing slots for '{decorator.GetType().Name}' to decorate, or decoration conditions not met.");
            return false;
        }
    }
}
