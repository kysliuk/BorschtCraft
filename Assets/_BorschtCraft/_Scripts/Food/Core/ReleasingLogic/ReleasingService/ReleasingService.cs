using Zenject;

namespace BorschtCraft.Food
{
    public class ReleasingService : ItemHandlableService
    {
        protected override void OnInitialize()
        {
            _signalBus.Subscribe<ReleaseSlotItemSignal>(OnReleaseSlotItem);
        }

        protected override void OnDispose()
        {
            _signalBus.TryUnsubscribe<ReleaseSlotItemSignal>(OnReleaseSlotItem);
        }

        private void OnReleaseSlotItem(ReleaseSlotItemSignal signal)
        {
            Logger.LogInfo(this, $"Received request to release item of type {signal.Slot.Item.Value.GetType().Name} from slot of type {signal.Slot.SlotType}.");
            _itemHandler.Handle(signal.Slot.Item.Value);
        }

        public ReleasingService(SignalBus signalBus, ISlotReleasingHandler itemHandler) : base(signalBus, itemHandler)
        {
        }
    }
}
