using BorschtCraft.Food.Signals;
using BorschtCraft.Food.UI;
using System.Linq;
using Zenject;

namespace BorschtCraft.Food
{
    public class ConsumingService : IConsumingService
    {
        private readonly SignalBus _signalBus;
        private readonly IItemSlot[] _cookingSlots;
        private readonly ICombiningService _combiningService;

        public void Initialize()
        {
            _signalBus.Subscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        private void OnConsumableInteractionRequested(ConsumableInteractionRequestSignal signal)
        {
            Logger.LogInfo(this, $"Received consumable interaction request signal for {signal.ConsumableSource.GetType().Name}.");
            var consumableSource = signal.ConsumableSource;

            Logger.LogInfo(this, $"{nameof(OnConsumableInteractionRequested)} for {consumableSource?.GetType().Name}");

            if (consumableSource == null) return;

            bool wasHandledByCombiner = false;

            if(CanBeDecorator(consumableSource))
            {
                Logger.LogInfo(this, $"{consumableSource.GetType().Name} might be a decorator. Attempting combination via CombiningService.");
                wasHandledByCombiner = _combiningService.AttemptCombination(consumableSource);
            }

            if (wasHandledByCombiner)
            {
                Logger.LogInfo(this, $"{consumableSource.GetType().Name} interaction was handled by CombiningService.");
                return;
            }

            Logger.LogInfo(this, $"{consumableSource.GetType().Name} not handled by Combiner or not a decorator. Attempting initial production.");

            var producedItem = consumableSource.Consume(null);

            if (producedItem != null)
            {
                var targetSlot = FindEmptyCookingSlot(); 

                if (targetSlot != null)
                {
                    targetSlot.TrySetItem(producedItem);
                    Logger.LogInfo(this, $"Produced {producedItem.GetType().Name} from {consumableSource.GetType().Name} into cooking slot {targetSlot.GetGameObject().name}."); // Changed access

                    if (producedItem is ICookable)
                    {
                        Logger.LogInfo(this, $"Auto-requesting cook for {producedItem.GetType().Name} in slot {targetSlot.GetGameObject().name}"); // Changed access
                        _signalBus.Fire(new CookItemInSlotRequestSignal(targetSlot as ItemSlotController));
                    }
                }
                else
                {
                    Logger.LogWarning(this, $"No empty cooking slot found for {producedItem.GetType().Name} from {consumableSource.GetType().Name}.");
                }
            }
            else
            {
                Logger.LogWarning(this, $"{consumableSource.GetType().Name}.Consume(null) did not produce an item. This is expected if it's purely a decorator.");
            }
        }

        private bool CanBeDecorator(IConsumable consumable)
        {
            return consumable is not ICantDecorate;
        }

        private IItemSlot FindEmptyCookingSlot() // Changed return type
        {
            if (_cookingSlots == null) return null;
            // slot is IItemSlot, access CurrentItemInSlot via GetCurrentItem()
            return _cookingSlots.FirstOrDefault(slot => slot != null && slot.GetCurrentItem() == null); // Changed access
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        public ConsumingService(SignalBus signalBus,
                                [Inject(Id = "CookingSlots")] IItemSlot[] cookingSlots, // Changed parameter type
                                ICombiningService combiningService)
        {
            _signalBus = signalBus;
            _cookingSlots = cookingSlots;
            _combiningService = combiningService;
        }
    }
}
