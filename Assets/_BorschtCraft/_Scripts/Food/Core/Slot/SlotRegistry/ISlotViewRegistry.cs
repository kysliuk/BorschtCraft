using BorschtCraft.Food.UI;

namespace BorschtCraft.Food
{
    public interface ISlotViewRegistry
    {
        SlotView[] SlotViews { get; }
        void Register(SlotView slotView);
        void Clear();
        SlotView GetSlotView(ISlot slot);
    }
}
