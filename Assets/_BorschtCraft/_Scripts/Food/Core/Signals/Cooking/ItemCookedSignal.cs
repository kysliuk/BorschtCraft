namespace BorschtCraft.Food.Signals
{
    public class ItemCookedSignal
    {
        public IConsumed Consumed { get; }
        public IItemSlot OriginSlot { get; }

        public ItemCookedSignal(IConsumed consumed, IItemSlot originSlot)
        {
            Consumed = consumed;
            OriginSlot = originSlot;
        }
    }
}