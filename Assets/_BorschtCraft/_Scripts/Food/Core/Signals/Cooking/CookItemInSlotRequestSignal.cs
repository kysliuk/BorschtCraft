namespace BorschtCraft.Food.Signals
{
    public class CookItemInSlotRequestSignal
    {
        public IItemSlot Slot { get; }

        public CookItemInSlotRequestSignal(IItemSlot slot)
        {
            Slot = slot;
        }
    }
}