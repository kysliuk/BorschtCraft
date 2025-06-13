using BorschtCraft.Food.Signals;
using System.Linq;
using Zenject;

namespace BorschtCraft.Food
{
    public class ItemTransferService : IItemTransferService
    {
        private readonly SignalBus _signalBus;
        private readonly IItemSlot[] _releasingSlots;

        public ItemTransferService(SignalBus signalBus, [Inject(Id = "ReleasingSlots")] IItemSlot[] releasingSlots)
        {
            _signalBus = signalBus;
            _releasingSlots = releasingSlots;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<SlotClickedSignal>(OnSlotClickedToMove);
        }

        private void OnSlotClickedToMove(SlotClickedSignal signal)
        {
            var clickedSlot = signal.ClickedSlot;

            // Logic is now based on the SlotType, not its GameObject identity.
            if (clickedSlot.Type == SlotType.Cooking && clickedSlot.CurrentItem.Value is ICooked)
            {
                var targetReleasingSlot = FindEmptyReleasingSlot();
                if (targetReleasingSlot == null)
                {
                    Logger.LogInfo(this, "No empty releasing slot available.");
                    return;
                }

                var itemToMove = clickedSlot.ReleaseItem();
                if (itemToMove != null)
                {
                    targetReleasingSlot.TrySetItem(itemToMove);
                    Logger.LogInfo(this, $"Moved {itemToMove.GetType().Name} from cooking slot to releasing slot.");
                }
            }
        }

        private IItemSlot FindEmptyReleasingSlot()
        {
            return _releasingSlots.FirstOrDefault(slot => slot.IsEmpty());
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<SlotClickedSignal>(OnSlotClickedToMove);
        }
    }
}