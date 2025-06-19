namespace BorschtCraft.Food
{
    public class ReleaseSlotItemSignal
    {
        public ISlot Slot { get; private set; }
        public IConsumed Consumed { get; private set; }
        public SlotType Type => Slot.SlotType;

        public ReleaseSlotItemSignal(ISlot slot, IConsumed consumed)
        {
            Slot = slot;
        }
    }
}
