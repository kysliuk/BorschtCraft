using BorschtCraft.Food.Signals;
using BorschtCraft.Food.UI;
using PlasticPipe.PlasticProtocol.Messages;
using System.Linq;
using Zenject;

namespace BorschtCraft.Food
{
    public class SelectedItemService : ISelectedItemService
    {
        public ItemSlotController CurrentSelectedSlot { get; private set; }

        public IConsumed CurrentSelectedItem => CurrentSelectedSlot?.CurrentItemInSlot;

        private readonly SignalBus _signalBus;
        private readonly ItemSlotController[] _releasingSlots;

        public void Initialize()
        {
            _signalBus.Subscribe<SlotClickedSignal>(OnSlotClicked);
        }

        public void SelectSlot(ItemSlotController slot)
        {
            CurrentSelectedSlot = slot;
        }

        public void Deselect()
        {
            CurrentSelectedSlot = null;
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<SlotClickedSignal>(OnSlotClicked);
        }

        private void OnSlotClicked(SlotClickedSignal signal)
        {
            if(_releasingSlots.Contains(signal.ClickedSlot) && signal.ClickedSlot.CurrentItemInSlot != null)
            {
                SelectSlot(signal.ClickedSlot);
                Logger.LogInfo(this, $"Item {signal.ClickedSlot.CurrentItemInSlot.GetType().Name} in slot {signal.ClickedSlot.gameObject.name} selected.");
            }
            else if(CurrentSelectedSlot == signal.ClickedSlot)
            {
                Deselect();
                Logger.LogInfo(this, $"Slot {signal.ClickedSlot.gameObject.name} deselected by clicking again.");
            }
        }

        public SelectedItemService(SignalBus signalBus, [Inject(Id = "ReleasingSlots")] ItemSlotController[] releasingSlots)
        {
            _signalBus = signalBus;
            _releasingSlots = releasingSlots;
        }
    }
}
