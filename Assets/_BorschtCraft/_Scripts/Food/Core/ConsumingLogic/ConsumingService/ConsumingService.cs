using BorschtCraft.Food.Signals;
using Zenject;

namespace BorschtCraft.Food
{
    public class ConsumingService : ItemHandlableService
    {
        protected override void OnInitialize()
        {
            _signalBus.Subscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        protected override void OnDispose()
        {
            _signalBus.TryUnsubscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        private void OnConsumableInteractionRequested(ConsumableInteractionRequestSignal signal)
        {
            Logger.LogInfo(this, $"Received request for {signal.ConsumableSource.GetType().Name}. Starting handler chain.");
            _itemHandler.Handle(signal.ConsumableSource);
        }

        public ConsumingService(SignalBus signalBus, IConsumingItemHandler itemHandler) : base(signalBus, itemHandler)
        {
        }
    }
}
