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
            Logger.LogInfo(this, $"{typeof(T).Name} Set parent slot view model: {_parentSlotViewModel?.GetHashCode()} for slot of type {slotViewModel?.Slot?.SlotType}.");
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        protected void OnSlotItemChangedSignal(SlotItemChangedSignal<T> signal)
        {
            Logger.LogInfo(this, $"{typeof(T).Name} Received SlotItemChangedSignal for slot of type {signal.Slot.SlotType} with item of type {typeof(T).Name}.");

            if (signal.SlotViewModel != _parentSlotViewModel)
            {
                Logger.LogWarning(this, $"{typeof(T).Name} Received signal for a different slot expected {signal.SlotViewModel.GetHashCode()} but was: {_parentSlotViewModel.GetHashCode()}. Ignoring signal.");
                return;
            }

            _consumedModel = signal.Slot.Item.Value;
            Logger.LogInfo(this, $"{typeof(T).Name} Setting consumed model to {_consumedModel?.GetType().Name ?? "null"} for slot of type {signal.Slot.SlotType}.");

            var hasModel = _consumedModel != null;
            Logger.LogInfo(this, $"{typeof(T).Name} Checking if consumed model is null: {hasModel} for slot of type {signal.Slot.SlotType} with item of type {typeof(T).Name}.");

            _isVisible.Value = hasModel;
            Logger.LogInfo(this, $"{typeof(T).Name} Setting visibility to {_isVisible.Value} for slot of type {signal.Slot.SlotType} with item of type {typeof(T).Name}.");
        }

        public ConsumedViewModel(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<SlotItemChangedSignal<T>>(OnSlotItemChangedSignal);
        }
    }
}
