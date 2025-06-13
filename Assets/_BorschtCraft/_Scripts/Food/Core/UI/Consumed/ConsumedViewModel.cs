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

        public ConsumedViewModel(T consumedModel, SignalBus signalBus)
        {
            _consumedModel = consumedModel;
            _signalBus = signalBus;
        }
    }
}