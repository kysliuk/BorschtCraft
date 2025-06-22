using Zenject;

namespace BorschtCraft.Food.UI
{
    public class ConsumableViewModel<T1, T2> where T1 : Consumable<T2> where T2 : Consumed
    {
        protected readonly T1 _consumableModel;
        protected readonly SignalBus _signalBus;

        public virtual void AttemptConsume()
        {
            Logger.LogInfo(this, $"Attempting to consume {_consumableModel.GetType().Name}.");
            _signalBus.Fire(new ConsumableInteractionRequestSignal(_consumableModel));
        }

        public ConsumableViewModel(T1 consumable, SignalBus signalBus)
        {
            _consumableModel = consumable;
            _signalBus = signalBus;
        }
    }
}
