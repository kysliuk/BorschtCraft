namespace BorschtCraft.Food
{
    public class CookItemInSlotSignal
    {
        public ISlot Slot { get; }

        public CookItemInSlotSignal(ISlot slot)
        {
            Slot = slot;
        }
    }
}
