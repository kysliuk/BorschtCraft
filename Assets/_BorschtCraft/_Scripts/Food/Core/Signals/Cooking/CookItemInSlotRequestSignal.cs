namespace BorschtCraft.Food.Signals
{
    public class CookItemInSlotRequestSignal
    {
        public IItemSlot Slot { get; } // Changed property name and type

        public CookItemInSlotRequestSignal(IItemSlot slot) // Changed parameter name and type
        {
            Slot = slot; // Updated assignment
        }
    }
}
