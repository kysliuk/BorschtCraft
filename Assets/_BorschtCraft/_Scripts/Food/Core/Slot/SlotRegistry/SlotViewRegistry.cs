using BorschtCraft.Food.UI;
using System.Collections.Generic;
using System.Linq;

namespace BorschtCraft.Food
{
    public class SlotViewRegistry : ISlotViewRegistry
    {
        public SlotView[] SlotViews => _slotViews.ToArray();

        private readonly List<SlotView> _slotViews = new();

        public void Clear() => _slotViews.Clear();

        public SlotView GetSlotView(ISlot slot)
        {
            return _slotViews.FirstOrDefault(sv => sv.SlotViewModel.Slot == slot);
        }

        public void Register(SlotView slotView)
        {
            if(!_slotViews.Contains(slotView))
                _slotViews.Add(slotView);
        }
    }
}
