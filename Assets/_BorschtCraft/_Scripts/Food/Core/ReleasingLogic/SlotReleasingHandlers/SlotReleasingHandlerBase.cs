using System;
using System.Linq;
using System.Threading.Tasks;
using UniRx;

namespace BorschtCraft.Food
{
    public abstract class SlotReleasingHandlerBase<T> : StrategizedItemHandler<T>, ISlotReleasingHandler where T : ISlotMatchingStrategy
    {
        protected IConsumed _consumed;
        private readonly ReplaySubject<ItemDeliveredSignal> _itemDeliveredSubject = new(1);

        private Action<ItemDeliveredSignal> _onItemDelivered;

        protected override bool CanHandle(IItem item)
        {
            if (item is not IConsumed c)
                return false;

            _consumed = c;
            return true;
        }

        protected override async Task<bool> Process(IItem item)
        {
            var slot = _slotRegistry.Slots.FirstOrDefault(s => _strategy.Matches(s, item) && s.Item.Value == item);
            if (slot != null)
            {
                Logger.LogInfo(this, $"Item of type {item.GetType().Name} matches slot of type {slot.SlotType}.");

                var released = await ProcessItemReleasing(slot);
                if (released)
                    slot.ClearCurrentItem();

                return released;
            }

            Logger.LogInfo(this, $"No matching slot for {item.GetType().Name}.");
            return false;
        }

        protected override void OnInitialize()
        {
            _signalBus.Subscribe<ReleaseSlotItemSignal>(OnReleaseSlotItemSignal);

            _onItemDelivered = signal => _itemDeliveredSubject.OnNext(signal);
            _signalBus.Subscribe(_onItemDelivered);
        }

        protected override void OnDispose()
        {
            _signalBus.TryUnsubscribe<ReleaseSlotItemSignal>(OnReleaseSlotItemSignal);

            if (_onItemDelivered != null)
                _signalBus.TryUnsubscribe(_onItemDelivered);
        }

        protected virtual async Task<bool> ProcessItemReleasing(ISlot slot)
        {
            var item = slot.Item.Value;
            var deliverySignal = new CustomerDeliverySignal(item);
            _signalBus.Fire(deliverySignal);

            Logger.LogInfo(this, $"Fired signal {nameof(CustomerDeliverySignal)} with DeliveryId: {deliverySignal.DeliveryId}");
            var deliveryResult = await _itemDeliveredSubject
                .First(signal => signal.DeliveryId == deliverySignal.DeliveryId)
                .ToTask();

            Logger.LogInfo(this, $"Delivery {deliveryResult.DeliveryId} completed {deliveryResult?.Delivered}");

            return deliveryResult.Delivered;
        }

        private void OnReleaseSlotItemSignal(ReleaseSlotItemSignal signal)
        {
            if (signal.Slot.SlotType == _strategy.SlotType)
                Handle(signal.Slot.Item.Value);
        }
    }
}
