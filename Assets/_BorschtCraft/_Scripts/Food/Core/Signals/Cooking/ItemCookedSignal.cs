using BorschtCraft.Food.UI;

namespace BorschtCraft.Food.Signals
{
    public class ItemCookedSignal
    {
        public IConsumed Consumed { get; }
        public ItemSlotController OriginSlot { get; }

        public ItemCookedSignal(IConsumed consumed,ItemSlotController originSlot)
        {
            Consumed = consumed;
            OriginSlot = originSlot;
        }
    }
}
