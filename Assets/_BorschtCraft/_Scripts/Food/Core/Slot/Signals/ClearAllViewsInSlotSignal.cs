using BorschtCraft.Food.UI;

namespace BorschtCraft.Food
{
    public class ClearAllViewsInSlotSignal
    {
        public SlotViewModel SlotViewModel { get; private set; }

        public ClearAllViewsInSlotSignal(SlotViewModel slotViewModel)
        {
            SlotViewModel = slotViewModel;
        }
    }
}
