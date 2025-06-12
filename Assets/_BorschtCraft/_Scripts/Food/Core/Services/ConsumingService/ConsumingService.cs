using BorschtCraft.Food.Signals;
using BorschtCraft.Food.UI;
using System.Linq;
using Zenject;

namespace BorschtCraft.Food
{
    public class ConsumingService : IConsumingService
    {
        private readonly SignalBus _signalBus;
        private readonly ItemSlotController[] _cookingSlots;
        private readonly ISelectedItemService _selectedItemService;

        public void Initialize()
        {
            _signalBus.Subscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        private void OnConsumableInteractionRequested(ConsumableInteractionRequestSignal signal)
        {
            Logger.LogInfo(this, $"Received consumable interaction request signal for {signal.ConsumableSource.GetType().Name}.");
            if (_selectedItemService.CurrentSelectedItem != null)
                return;

            var targetSlot = FindEmptyCookingSlot();
            if (targetSlot == null)
                return;

            IConsumable consumableSource = signal.ConsumableSource;

            IConsumed producedItem = consumableSource.Consume(null);

            var itemSetted = targetSlot.TrySetItem(producedItem);
            Logger.LogInfo(this, $"Produced {producedItem.GetType().Name} from {consumableSource.GetType().Name} into cooking slot {targetSlot.gameObject.name}.");

            if (producedItem is ICookable && itemSetted)
                _signalBus.Fire(new CookItemInSlotRequestSignal(targetSlot));

        }

        private ItemSlotController FindEmptyCookingSlot()
        {
            return _cookingSlots.FirstOrDefault(slot => slot.CurrentItemInSlot == null);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        public ConsumingService(SignalBus signalBus,
                                [Inject(Id = "CookingSlots")] ItemSlotController[] cookingSlots,
                                ISelectedItemService selectedItemService)
        {
            _signalBus = signalBus;
            _cookingSlots = cookingSlots;
            _selectedItemService = selectedItemService;
        }
    }
}
