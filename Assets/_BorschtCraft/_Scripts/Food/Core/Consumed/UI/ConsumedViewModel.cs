using System;
using UniRx;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class ConsumedViewModel<T> : IDisposable where T : IConsumed
    {
        public IReadOnlyReactiveProperty<bool> IsVisible => _isVisible;
        public Type ModelType => typeof(T);

        protected IConsumed _consumedModel;
        protected SignalBus _signalBus;

        protected ReactiveProperty<bool> _isVisible = new ReactiveProperty<bool>(false);
        protected CompositeDisposable _disposables = new CompositeDisposable();
        protected SlotViewModel _parentSlotViewModel;

        public void SetParentSlotViewModel(SlotViewModel slotViewModel)
        {
            _parentSlotViewModel = slotViewModel;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        protected virtual void SetVisibility(bool visible)
        {
            _isVisible.Value = visible;
        }

        protected void OnSlotItemChangedSignal(SlotItemChangedSignal<T> signal)
        {
            if (signal.Slot != _parentSlotViewModel.Slot)
                return;

            _consumedModel = signal.Slot.Item.Value;
            if (_consumedModel == null)
            {
                SetVisibility(false);
                return;
            }
        }

        public ConsumedViewModel(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<SlotItemChangedSignal<T>>(OnSlotItemChangedSignal);
        }
    }
}
