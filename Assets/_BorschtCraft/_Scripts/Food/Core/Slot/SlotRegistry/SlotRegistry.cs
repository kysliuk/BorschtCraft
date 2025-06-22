using System.Collections.Generic;
using System.Linq;

namespace BorschtCraft.Food
{
    public class SlotRegistry : ISlotRegistry
    {
        public ISlot[] Slots => _slots.ToArray();

        private readonly List<ISlot> _slots = new();

        public void Register(ISlot slot)
        {
            if (!_slots.Contains(slot))
            {
                _slots.Add(slot);
                Logger.LogInfo(this, $"Slot of type {slot.SlotType} were added to registry with {slot.Item} item.");
            }
        }

        public void Clear() => _slots.Clear();

        public ISlot GetEmptySlot(SlotType? slotType = null)
        {
            return _slots.FirstOrDefault(s => s.Item.Value == null && (slotType == null || s.SlotType == slotType));
        }
    }
}
