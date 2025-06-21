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

        public virtual void ReleaseItem()
        {
            _signalBus.Fire(new ReleaseSlotItemSignal(_slotModel));
        }

        public virtual void Dispose()
        {
            _disposables.Dispose();
        }

        private void OnItemSlotChanged(IConsumed item)
        {
            var type = item?.GetType();

            if (type != null)
            {
                var signalType = typeof(SlotItemChangedSignal<>).MakeGenericType(type);
                var signalInstance = Activator.CreateInstance(signalType, Slot, this);
                _signalBus.Fire(signalInstance);
            }
            else
            {
                var signalInstance = new ClearAllViewsInSlotSignal(this);
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
