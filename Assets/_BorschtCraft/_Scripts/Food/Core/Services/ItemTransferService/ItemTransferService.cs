using BorschtCraft.Food.Signals;
using System.Linq;
using Zenject;

namespace BorschtCraft.Food
{
    public class ItemTransferService : IItemTransferService
    {
        private readonly SignalBus _signalBus;
        private readonly IItemSlot[] _cookingSlots;
        private readonly IItemSlot[] _releasingSlots;

        public void Initialize()
        {
            _signalBus.Subscribe<SlotClickedSignal>(OnSlotClickedToMove);
        }

        private void OnSlotClickedToMove(SlotClickedSignal signal)
        {
            var clickedSlot = signal.ClickedSlot;
            if (clickedSlot == null)
                return;

            var targetReleasingSlot = FindEmptyReleasingSlot(); 
            if (targetReleasingSlot == null)
            {
                Logger.LogInfo(this, "OnSlotClickedToMove: No empty releasing slot available.");
                return;
            }

            bool isClickedSlotACookingSlot = _cookingSlots.Any(s => s.GetGameObject() == clickedSlot.GetGameObject());

            if (isClickedSlotACookingSlot && clickedSlot.GetCurrentItem() is ICooked)
            {
                var consumedToMove = clickedSlot.ReleaseItem();

                if (consumedToMove != null)
                {
                    targetReleasingSlot.TrySetItem(consumedToMove);
                    Logger.LogInfo(this, $"Moved {consumedToMove.GetType().Name} from cooking slot {clickedSlot.GetGameObject().name} to releasing slot {targetReleasingSlot.GetGameObject().name}.");
                }
                else
                {
                    Logger.LogWarning(this, $"Attempted to move from {clickedSlot.GetGameObject().name}, but ReleaseItem() returned null.");
                }
            }
        }

        private IItemSlot FindEmptyReleasingSlot()
        {
            return _releasingSlots.FirstOrDefault(slot => slot != null && slot.GetCurrentItem() == null);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<SlotClickedSignal>(OnSlotClickedToMove);
        }

        public ItemTransferService(SignalBus signalBus,
                                   [Inject(Id = "CookingSlots")] IItemSlot[] cookingSlots,
                                   [Inject(Id = "ReleasingSlots")] IItemSlot[] releasingSlots)
        {
            _signalBus = signalBus;
            _cookingSlots = cookingSlots;
            _releasingSlots = releasingSlots;
        }
    }
}
