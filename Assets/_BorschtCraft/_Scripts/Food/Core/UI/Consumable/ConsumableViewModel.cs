using BorschtCraft.Food.Signals;
using UniRx;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public abstract class ConsumableViewModel<T1, T2> where T1 : Consumable<T2> where T2 : Consumed
    {
        public IReadOnlyReactiveProperty<string> DisplayName => _displayName;
        public IReadOnlyReactiveProperty<bool> CanBeConsumed => _canBeConsumed;

        protected readonly T1 _consumableModel;
        protected readonly SignalBus _signalBus;

        protected readonly ReactiveProperty<string> _displayName = new ReactiveProperty<string>();
        protected readonly ReactiveProperty<bool> _canBeConsumed = new ReactiveProperty<bool>(true);

        public virtual void AttemptConsume()
        {
            Logger.LogInfo(this, $"Attempting to consume {_consumableModel.GetType().Name}. Can consume {_canBeConsumed.Value}");
            if (_canBeConsumed.Value)
            {
                var signal = new ConsumableInteractionRequestSignal<T1, T2>(_consumableModel) as IConsumableInteractionRequestSignal;
                Logger.LogInfo(this, $"Firing signal: {signal.GetType().Name} for consumable {_consumableModel.GetType().Name}");
                _signalBus.Fire(signal);
            }
        }

        public ConsumableViewModel(T1 consumable, SignalBus signalBus)
        {
            _consumableModel = consumable;
            _signalBus = signalBus;
            _displayName.Value = _consumableModel.GetType().Name;
        }
    }
}
