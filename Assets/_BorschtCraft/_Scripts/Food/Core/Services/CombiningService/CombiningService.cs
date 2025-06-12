using BorschtCraft.Food.Signals;
using Zenject;

namespace BorschtCraft.Food
{
    public class CombiningService : ICombiningService
    {
        private readonly SignalBus _signalBus;
        private readonly ISelectedItemService _selectedItemService;

        public void Initialize()
        {
            Logger.LogInfo(this, "initialized and ready to process consumable interactions.");
            _signalBus.Subscribe<IConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        private void OnConsumableInteractionRequested(IConsumableInteractionRequestSignal signal)
        {
            Logger.LogInfo(this, $"Received consumable interaction request signal for {signal.ConsumableSource.GetType().Name}.");
            //var selectedSlot = _selectedItemService.CurrentSelectedSlot;
            //if (selectedSlot == null || selectedSlot.CurrentItemInSlot == null)
            //    return;

            //var consumable = signal.ConsumableSource;
            //var consumedToDecorate = selectedSlot.CurrentItemInSlot;

            //var consumedDecorated = consumable.Consume(consumedToDecorate);

            //if(consumedDecorated != null && consumedDecorated != consumedToDecorate)
            //{
            //    selectedSlot.TrySetItem(consumedDecorated);
            //    Logger.LogInfo(this, $"{consumedToDecorate.GetType().Name} in slot {selectedSlot.gameObject.name} decorated with {consumable.GetType().Name}. New top layer: {consumedDecorated.GetType().Name}");
            //}
            //else if (consumedDecorated == consumedToDecorate)
            //{
            //    Logger.LogInfo(this, $"Attempted decoration of {consumedToDecorate.GetType().Name} with {consumable.GetType().Name} resulted in no change. Target might be invalid or decorator inapplicable.");
            //}
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<IConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        public CombiningService(SignalBus signalBus, ISelectedItemService selectedItemService)
        {
            _signalBus = signalBus;
            _selectedItemService = selectedItemService;
        }
    }
}
