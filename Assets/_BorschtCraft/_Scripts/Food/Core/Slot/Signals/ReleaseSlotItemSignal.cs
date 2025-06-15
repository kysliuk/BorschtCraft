namespace BorschtCraft.Food
{
    public class ReleaseSlotItemSignal
    {
        public ISlot Slot { get; private set; }
        public SlotType Type => Slot.SlotType;

        public ReleaseSlotItemSignal(ISlot slot)
        {
            Slot = slot;
        }
    }
}
