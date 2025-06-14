using BorschtCraft.Food.Signals;
using System.Linq;
using Zenject;

namespace BorschtCraft.Food.Core.Services.ConsumingService.Strategies
{
    public class InitialProductionStrategy : IConsumptionStrategy
    {
        private readonly SignalBus _signalBus;

        public InitialProductionStrategy(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public bool TryExecute(IConsumable consumableSource,
                                 IItemSlot[] availableCookingSlots,
                                 IItemSlot[] availableReleasingSlots,
                                 out IConsumed finalItem,
                                 out IItemSlot finalSlot)
        {
            finalItem = null;
            finalSlot = null;

            if (consumableSource.CanDecorate(null))
            {
                var producedItem = consumableSource.Consume(null);

                if (producedItem != null)
                {
                    var targetSlot = availableCookingSlots?.FirstOrDefault(slot => slot != null && slot.IsEmpty());

                    if (targetSlot != null)
                    {
                        targetSlot.TrySetItem(producedItem);
                        Logger.LogInfo(this, $"Produced {producedItem.GetType().Name} from {consumableSource.GetType().Name} into cooking slot {targetSlot.GetGameObject().name}.");
                        finalItem = producedItem;
                        finalSlot = targetSlot;

                        if (producedItem is ICookable)
                        {
                            Logger.LogInfo(this, $"Auto-requesting cook for {producedItem.GetType().Name} in slot {targetSlot.GetGameObject().name}");
                            _signalBus.Fire(new CookItemInSlotRequestSignal(targetSlot));
                        }
                        return true;
                    }
                    else
                    {
                        Logger.LogWarning(this, $"No empty cooking slot found for {producedItem.GetType().Name} from {consumableSource.GetType().Name}.");
                    }
                }
                else
                {
                   Logger.LogInfo(this, $"{consumableSource.GetType().Name}.Consume(null) did not produce an item.");
                }
            }
            return false;
        }
    }
}
