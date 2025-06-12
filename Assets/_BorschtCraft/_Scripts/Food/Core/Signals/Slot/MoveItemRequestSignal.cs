using BorschtCraft.Food.UI;

namespace BorschtCraft.Food.Signals
{
    public class MoveItemRequestSignal
    {
        public ItemSlotController SourceSlot { get; }

        public MoveItemRequestSignal(ItemSlotController sourceSlot)
        {
            SourceSlot = sourceSlot;
        }
    }
}
