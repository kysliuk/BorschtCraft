using System;
using UniRx;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class ConsumedViewModel
    {
        public IReadOnlyReactiveProperty<bool> IsVisible => _isVisible;
        public Type ModelType => _consumedModel.GetType();

        protected IConsumed _consumedModel;
        protected SignalBus _signalBus;

        protected ReactiveProperty<bool> _isVisible = new ReactiveProperty<bool>(false);

        public virtual void SetVisibility(bool visible)
        {
            _isVisible.Value = visible;
        }

        public ConsumedViewModel(IConsumed consumedModel, SignalBus signalBus)
        {
            _consumedModel = consumedModel;
            _signalBus = signalBus;
        }
    }
}
