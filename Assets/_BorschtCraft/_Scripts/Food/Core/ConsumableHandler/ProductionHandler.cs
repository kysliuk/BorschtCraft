using BorschtCraft.Food.Signals;
using System.Linq;
using Zenject;

namespace BorschtCraft.Food.Handlers
{
    public class ProductionHandler : ConsumableHandlerBase
    {
        private readonly SignalBus _signalBus;
        private readonly IItemSlot[] _cookingSlots;

        protected override bool CanHandle(IConsumable consumable)
        {
            return true;
        }

        protected override bool Process(IConsumable consumable)
        {
            Logger.LogInfo(this, "Attempting initial production.");
            var producedItem = consumable.Consume(null);

            if (producedItem == null)
            {
                Logger.LogWarning(this, $"{consumable.GetType().Name}.Consume(null) did not produce an item. This is expected if it's purely a decorator.");
                return false;
            }

            var targetSlot = FindEmptyCookingSlot();
            if (targetSlot != null)
            {
                targetSlot.TrySetItem(producedItem);
                Logger.LogInfo(this, $"Produced {producedItem.GetType().Name} into a cooking slot.");

                if (producedItem is ICookable)
                {
                    Logger.LogInfo(this, $"Auto-requesting cook for {producedItem.GetType().Name}.");
                    _signalBus.Fire(new CookItemInSlotRequestSignal(targetSlot));
                }
                return true;
            }

            Logger.LogWarning(this, $"No empty cooking slot found for {producedItem.GetType().Name}.");
            return false;
        }

        private IItemSlot FindEmptyCookingSlot()
        {
            Logger.LogWarning(this, $"Searching for an empty cooking slot... {_cookingSlots.Length}");
            return _cookingSlots.FirstOrDefault(slot => slot.IsEmpty());
        }

        public ProductionHandler(
            SignalBus signalBus,
            [Inject(Id = "CookingSlots")] IItemSlot[] cookingSlots)
        {
            _signalBus = signalBus;
            _cookingSlots = cookingSlots;
        }
    }
}