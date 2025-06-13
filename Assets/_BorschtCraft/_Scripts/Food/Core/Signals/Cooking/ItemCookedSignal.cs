using BorschtCraft.Food.UI;
using BorschtCraft.Food.Core.Interfaces; // Added

namespace BorschtCraft.Food.Signals
{
    public class ItemCookedSignal
    {
        public IConsumed Consumed { get; }
        public IItemSlot OriginSlot { get; } // Changed type

        public ItemCookedSignal(IConsumed consumed, IItemSlot originSlot) // Changed parameter type
        {
            Consumed = consumed;
            OriginSlot = originSlot; // Assignment is fine
        }
    }
}
