using BorschtCraft.Food.Signals;
using Zenject;

namespace BorschtCraft.Food
{
    public class ItemConsumptionService : IItemConsumptionService
    {
        private readonly SignalBus _signalBus;

        public void Initialize()
        {
            Subscribe<BreadStack, BreadRaw>();
            Subscribe<SaloStack, Salo>();
            Subscribe<GarlicStack, Garlic>();
            Subscribe<OnionStack, Onion>();
            Subscribe<HorseradishStack, Horseradish>();
            Subscribe<MustardStack, Mustard>();
        }

        private void Subscribe<T1, T2>() where T1 : Consumable<T2> where T2 : Consumed
        {
            _signalBus.Subscribe<ConsumableInteractionRequestSignal<T1, T2>>(OnConsumableInteractionRequested);
            Logger.LogInfo(this, $"Subscribed to {typeof(T1).Name} consumption requests.");
        }

        private void OnConsumableInteractionRequested<T1, T2>(
        ConsumableInteractionRequestSignal<T1, T2> signal)
        where T1 : Consumable<T2>
        where T2 : Consumed
        {
            HandleConsumption<T1, T2>(signal.ConsumableSource);
        }


        private void HandleConsumption<T1, T2>(T1 consumableModel)
        where T1 : Consumable<T2>
        where T2 : Consumed
        {
            Logger.LogInfo(this, $"ItemConsumptionService: Received request for {consumableModel.GetType().Name}");
        }

        public ItemConsumptionService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
    }
}
