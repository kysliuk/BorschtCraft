using BorschtCraft.Food.UI;
using BorschtCraft.Food.Core.Interfaces; // Added

namespace BorschtCraft.Food.Signals
{
    public class SlotClickedSignal
    {
        public IItemSlot ClickedSlot { get; } // Changed type

        public SlotClickedSignal(IItemSlot clickedSlot) // Changed parameter type
        {
            ClickedSlot = clickedSlot; // Assignment is fine
        }
    }
}
