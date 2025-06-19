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
            Logger.LogInfo(this, $"Item of type {type?.Name} was set in slot of type {Slot?.SlotType} - {Slot.GetHashCode()}.");

            if (type != null)
            {
                var signalType = typeof(SlotItemChangedSignal<>).MakeGenericType(type);
                var signalInstance = Activator.CreateInstance(signalType, Slot, this);

                Logger.LogInfo(this, $"Firing signal of type {signalType.Name} for slot of type {Slot?.SlotType} with item of type {type.Name}.");
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

            Logger.LogInfo(this, $"Constructed SlotViewModel for slot of type {slotModel?.SlotType} with hash code {slotModel?.GetHashCode()}.");
        }
    }
}
