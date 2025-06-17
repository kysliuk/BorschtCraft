using System;
using UniRx;
using Zenject;

namespace BorschtCraft.Food.UI
{
    public class SlotViewModel : IDisposable
    {
        public IReadOnlyReactiveProperty<IConsumed> CurrentItem => _slotModel.Item;
        public ISlot Slot => _slotModel;

        protected readonly ISlot _slotModel;
        protected readonly SignalBus _signalBus;

        protected CompositeDisposable _disposables = new CompositeDisposable();

        public virtual void SetItem(IConsumed item)
        {
            if (item == null)
            {
                Logger.LogWarning(this, "Attempted to set a null item in the slot.");
                return;
            }

            _slotModel.SetItem(item);
        }

        public virtual void ReleaseItem()
        {
            _slotModel.Release();
            _signalBus.Fire(new ReleaseSlotItemSignal(_slotModel));
        }

        public virtual void Dispose()
        {
            _disposables.Dispose();
        }

        private void OnItemSlotChanged(IConsumed item)
        {
            var type = _slotModel.Item.Value?.GetType();

            if (type != null)
            {
                var signalType = typeof(SlotItemChangedSignal<>).MakeGenericType(type);
                var signalInstance = Activator.CreateInstance(signalType, _slotModel);
                _signalBus.Fire(signalInstance);
            }
        }

        public SlotViewModel(ISlot slotModel, SignalBus signalBus)
        {
            _slotModel = slotModel;
            _signalBus = signalBus;

            _slotModel.Item.Subscribe(OnItemSlotChanged).AddTo(_disposables);
        }
    }
}
