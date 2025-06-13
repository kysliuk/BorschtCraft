using BorschtCraft.Food.Signals;
using BorschtCraft.Food.Handlers;
using Zenject;

namespace BorschtCraft.Food
{
    public class ConsumingService : IConsumingService
    {
        private readonly SignalBus _signalBus;
        private readonly IConsumableHandler _handlerChain;

        public ConsumingService(SignalBus signalBus, IConsumableHandler handlerChain)
        {
            _signalBus = signalBus;
            _handlerChain = handlerChain;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }

        private void OnConsumableInteractionRequested(ConsumableInteractionRequestSignal signal)
        {
            Logger.LogInfo(this, $"Received request for {signal.ConsumableSource.GetType().Name}. Starting handler chain.");
            _handlerChain.Handle(signal.ConsumableSource);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<ConsumableInteractionRequestSignal>(OnConsumableInteractionRequested);
        }
    }
}