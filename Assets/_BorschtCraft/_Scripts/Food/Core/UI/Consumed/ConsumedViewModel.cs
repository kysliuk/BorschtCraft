using UniRx;
using Zenject;
using BorschtCraft.Food.Signals;
namespace BorschtCraft.Food.UI
{
    public class ConsumedViewModel<T> : IConsumedViewModel where T : Consumed
    {
        public IReadOnlyReactiveProperty<bool> IsVisible => _isVisible;
        protected T _consumedModel;
        protected SignalBus _signalBus;

        protected ReactiveProperty<bool> _isVisible = new ReactiveProperty<bool>(false);

        public virtual void SetVisibility(bool visible)
        {
            _isVisible.Value = visible;
        }

        //protected virtual void SubscribeToInteractionSignals<TConsumable, TConsumed>() where TConsumable : Consumable<TConsumed> where TConsumed : Consumed
        //{
        //    _signalBus.Subscribe<ConsumableInteractionRequestSignal<TConsumable, TConsumed>>(OnConsumableInteractionRequestSignal);
        //}

        //protected virtual void OnConsumableInteractionRequestSignal<TConsumable, TConsumed>(ConsumableInteractionRequestSignal<TConsumable, TConsumed> signal)
        //    where TConsumable : Consumable<TConsumed>
        //    where TConsumed : Consumed
        //{
        //    if (signal.ConsumableSource is TConsumed consumable && consumable == _consumedModel)
        //    {
        //        _isVisible.Value = true;
        //        signal.ConsumableSource.Consume(_consumedModel);
        //    }
        //}

        //public virtual void Dispose()
        //{

        //}

        public ConsumedViewModel(T consumedModel, SignalBus signalBus)
        {
            _consumedModel = consumedModel;
            _signalBus = signalBus;
        }
    }
}