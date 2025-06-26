using System;
using System.Linq;
using System.Threading.Tasks;
using UniRx;

namespace BorschtCraft.Food
{
    public abstract class SlotReleasingHandlerBase<T> : StrategizedItemHandler<T>, ISlotReleasingHandler where T : ISlotMatchingStrategy
    {
        protected IConsumed _consumed;
        private readonly Subject<ItemDeliveredSignal> _itemDeliveredSubject = new Subject<ItemDeliveredSignal>();

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
            _signalBus.Subscribe<ItemDeliveredSignal>(signal =>
            {
                _itemDeliveredSubject.OnNext(signal);
            });
        }

        protected override void OnDispose()
        {
            _signalBus.Unsubscribe<ReleaseSlotItemSignal>(OnReleaseSlotItemSignal);
            _signalBus.Unsubscribe<ItemDeliveredSignal>(signal =>
            {
                _itemDeliveredSubject.OnNext(signal);
            });
        }

        protected virtual async Task<bool> ProcessItemReleasing(ISlot slot)
        {
            var item = slot.Item.Value;

            var deliverySignal = new CustomerDeliverySignal(item);
            _signalBus.Fire(deliverySignal);

            var deliveryResult = await _itemDeliveredSubject
                .Where(signal => signal.DeliverySignal.Item == item)
                .First()
                .ToTask();

            return deliveryResult.Delivered;
        }

        private void OnReleaseSlotItemSignal(ReleaseSlotItemSignal signal)
        {
            Logger.LogInfo(this, $"Received request to release item from slot of type {signal.Slot.SlotType}.");

            if (signal.Slot.SlotType == _strategy.SlotType)
                Handle(signal.Slot.Item.Value);
        }
    }
}
