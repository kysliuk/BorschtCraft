using BorschtCraft.Food.Signals;
using System.Linq;
using Zenject;

namespace BorschtCraft.Food
{
    public class ItemTransferService : IItemTransferService
    {
        private readonly SignalBus _signalBus;
        private readonly IItemSlot[] _cookingSlots; // Changed type
        private readonly IItemSlot[] _releasingSlots; // Changed type
        // private readonly ISelectedItemService _selectedItemService; // Removed

        public void Initialize()
        {
            _signalBus.Subscribe<SlotClickedSignal>(OnSlotClickedToMove);
        }

        private void OnSlotClickedToMove(SlotClickedSignal signal)
        {
            var clickedSlot = signal.ClickedSlot; // This is now IItemSlot
            if (clickedSlot == null)
                return;

            var targetReleasingSlot = FindEmptyReleasingSlot(); // returns IItemSlot
            if (targetReleasingSlot == null)
            {
                Logger.LogInfo(this, "OnSlotClickedToMove: No empty releasing slot available.");
                return;
            }

            // Compare GameObjects since clickedSlot is IItemSlot and _cookingSlots contains IItemSlot
            bool isClickedSlotACookingSlot = _cookingSlots.Any(s => s.GetGameObject() == clickedSlot.GetGameObject());

            if (isClickedSlotACookingSlot && clickedSlot.GetCurrentItem() is ICooked)
            {
                // Removed _selectedItemService.Deselect() logic

                var consumedToMove = clickedSlot.ReleaseItem(); // ReleaseItem is on IItemSlot

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
            // else: Slot clicked was not a cooking slot with a cooked item, or was not a cooking slot at all.
        }

        private IItemSlot FindEmptyReleasingSlot() // Return type is already IItemSlot
        {
            // slot is IItemSlot, use GetCurrentItem()
            return _releasingSlots.FirstOrDefault(slot => slot != null && slot.GetCurrentItem() == null);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<SlotClickedSignal>(OnSlotClickedToMove);
        }

        public ItemTransferService(SignalBus signalBus,
                                   [Inject(Id = "CookingSlots")] IItemSlot[] cookingSlots, // Changed parameter type
                                   [Inject(Id = "ReleasingSlots")] IItemSlot[] releasingSlots // Changed parameter type
                                   /* ISelectedItemService selectedItemService Removed */ )
        {
            _signalBus = signalBus;
            _cookingSlots = cookingSlots;
            _releasingSlots = releasingSlots;
            // this._selectedItemService = selectedItemService; // Removed
        }
    }
}
