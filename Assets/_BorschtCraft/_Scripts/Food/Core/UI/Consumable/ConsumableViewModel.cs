using BorschtCraft.Food.Signals;
using UniRx;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class ConsumableViewModel<T1, T2> where T1 : Consumable<T2> where T2 : Consumed
    {
        private readonly T1 _consumableModel;
        private readonly SignalBus _signalBus;

        public IReadOnlyReactiveProperty<string> DisplayName => _displayName;
        private readonly ReactiveProperty<string> _displayName = new ReactiveProperty<string>();

        public IReadOnlyReactiveProperty<bool> CanBeConsumed => _canBeConsumed;
        private readonly ReactiveProperty<bool> _canBeConsumed = new ReactiveProperty<bool>(true);

        public void AttemptConsume()
        {
            Logger.LogInfo(this, $"Attempting to consume {_consumableModel.GetType().Name}. Can consume {_canBeConsumed.Value}");
            if (_canBeConsumed.Value)
            {
                _signalBus.Fire(new ConsumableInteractionRequestSignal<T1, T2>(_consumableModel));
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
