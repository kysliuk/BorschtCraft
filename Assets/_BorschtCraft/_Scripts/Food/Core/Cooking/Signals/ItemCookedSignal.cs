namespace BorschtCraft.Food
{
    public class ItemCookedSignal
    {
        public ISlot Slot { get; }

        public ItemCookedSignal(ISlot slot)
        {
            Slot = slot;
        }
    }
}
