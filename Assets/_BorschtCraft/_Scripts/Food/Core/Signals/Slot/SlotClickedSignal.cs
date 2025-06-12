using BorschtCraft.Food.UI;

namespace BorschtCraft.Food.Signals
{
    public class SlotClickedSignal
    {
        public ItemSlotController ClickedSlot { get; }

        public SlotClickedSignal(ItemSlotController clickedSlot)
        {
            ClickedSlot = clickedSlot;
        }
    }
}
