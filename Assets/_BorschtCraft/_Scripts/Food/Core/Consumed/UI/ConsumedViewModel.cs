using System;
using UniRx;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class ConsumedViewModel<T> : IDisposable where T : IConsumed
    {
        public IReadOnlyReactiveProperty<bool> IsVisible => _isVisible;

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

        protected void OnSlotItemChangedSignal(SlotItemChangedSignal<T> signal)
        {
            if (signal.SlotViewModel != _parentSlotViewModel)
            {
                Logger.LogInfo(this, $"{typeof(T).Name} Received signal for a different slot expected {signal.SlotViewModel.GetHashCode()} but was: {_parentSlotViewModel.GetHashCode()}. Ignoring signal.");
                return;
            }

            _consumedModel = signal.Slot.Item.Value;
            var hasModel = _consumedModel != null;
            _isVisible.Value = hasModel;
            Logger.LogInfo(this, $"{typeof(T).Name} Setting visibility to {_isVisible.Value} for slot of type {signal.Slot.SlotType} with item of type {typeof(T).Name}.");
        }

        protected void OnClearAllViewsInSlotSignal(ClearAllViewsInSlotSignal signal)
        {
            if (signal.SlotViewModel != _parentSlotViewModel)
                return;

            Logger.LogInfo(this, $"{typeof(T).Name} Clearing all views in slot of type {signal.SlotViewModel.Slot?.SlotType}.");
            _isVisible.Value = false;
        }

        public ConsumedViewModel(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<SlotItemChangedSignal<T>>(OnSlotItemChangedSignal);
            _signalBus.Subscribe<ClearAllViewsInSlotSignal>(OnClearAllViewsInSlotSignal);
        }
    }
}
