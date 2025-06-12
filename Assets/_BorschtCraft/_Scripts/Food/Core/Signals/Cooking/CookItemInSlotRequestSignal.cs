using BorschtCraft.Food.UI;

namespace BorschtCraft.Food.Signals
{
    public class CookItemInSlotRequestSignal
    {
        public ItemSlotController SlotController { get; }

        public CookItemInSlotRequestSignal(ItemSlotController slotController)
        {
            SlotController = slotController;
        }
    }
}
