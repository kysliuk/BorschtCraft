namespace BorschtCraft.Food
{
    public class StopCookinItemInSlotSignal
    {
        public ISlot Slot { get; }

        public StopCookinItemInSlotSignal(ISlot slot)
        {
            Slot = slot;
        }
    }
}
