using BorschtCraft.Food.Signals;
using BorschtCraft.Food.UI;
using System.Linq;
using Zenject;

namespace BorschtCraft.Food
{
    public class ItemTransferService : IItemTransferService
    {
        private readonly SignalBus _signalBus;
        private readonly IItemSlot[] _cookingSlots;
        private readonly IItemSlot[] _releasingSlots;
        private readonly ISelectedItemService _selectedItemService;

        public void Initialize()
        {
            _signalBus.Subscribe<SlotClickedSignal>(OnSlotClickedToMove);
        }

        private void OnSlotClickedToMove(SlotClickedSignal signal)
        {
            Logger.LogInfo(this, $"{nameof(OnSlotClickedToMove)} for {signal.ClickedSlot?.GetGameObject()?.name}");
            var clickedSlot = signal.ClickedSlot;
            if (clickedSlot == null)
                return;

            var targetReleasingSlot = FindEmptyReleasingSlot();
            if (targetReleasingSlot == null)
                return;


            if (_cookingSlots.Contains(clickedSlot) && clickedSlot.GetCurrentItem() is ICooked)
            {
                if(_selectedItemService.CurrentSelectedSlot == clickedSlot)
                    _selectedItemService.Deselect();

                var consumedToMove = clickedSlot.ReleaseItem();

                targetReleasingSlot.TrySetItem(consumedToMove);
                Logger.LogInfo(this, $"Moved {consumedToMove.GetType().Name} from cooking slot {clickedSlot.GetGameObject()?.name} to releasing slot {targetReleasingSlot.GetGameObject().name}.");
            }
        }

        private IItemSlot FindEmptyReleasingSlot()
        {
            return _releasingSlots.FirstOrDefault(slot => slot.GetCurrentItem() == null);
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<SlotClickedSignal>(OnSlotClickedToMove);
        }

        public ItemTransferService(SignalBus signalBus,
                                   [Inject(Id = "CookingSlots")] IItemSlot[] cookingSlots,
                                   [Inject(Id = "ReleasingSlots")] IItemSlot[] releasingSlots,
                                   ISelectedItemService selectedItemService)
        {
            _signalBus = signalBus;
            _cookingSlots = cookingSlots;
            _releasingSlots = releasingSlots;
            _selectedItemService = selectedItemService;
        }
    }
}
