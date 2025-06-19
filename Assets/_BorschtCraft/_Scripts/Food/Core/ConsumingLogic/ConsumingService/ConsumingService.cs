using BorschtCraft.Food.Signals;
using Zenject;

namespace BorschtCraft.Food
{
    public class ConsumingService : IConsumingService
    {
        private readonly SignalBus _signalBus;
        private IItemHandler _itemHandler;

        public void Initialize()
        {
            _signalBus.Subscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        private void OnConsumableInteractionRequested(ConsumableInteractionRequestSignal signal)
        {
            Logger.LogInfo(this, $"Received request for {signal.ConsumableSource.GetType().Name}. Starting handler chain.");
            _itemHandler.Handle(signal.ConsumableSource);
        }

        public ConsumingService(SignalBus signalBus, IConsumingItemHandler itemHandler)
        {
            _signalBus = signalBus;
            _itemHandler = itemHandler;
        }
    }
}
