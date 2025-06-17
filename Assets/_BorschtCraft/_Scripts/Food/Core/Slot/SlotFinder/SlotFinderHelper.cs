using System.Linq;
using Zenject;

namespace BorschtCraft.Food
{
    public static class SlotFinderHelper
    {
        private static ISlot[] _slots;

        public static void Initialize(ISlot[] slots)
        {
            _slots = slots;
        }

        public static bool TryFindSlot(IConsumable consumable, out ISlot slot)
        {
            return TryFindCombiningSlot(consumable, out slot);
        }

        private static bool TryFindCookingSlot(IConsumable consumable, out ISlot slot)
        {
            consumable.TryConsume(null, out var consumed);

            if (consumed is ICookable)
            {
                slot = _slots.FirstOrDefault(s => s.SlotType == SlotType.Cooking && s.Item.Value == null);
                return slot != null;
            }

            return TryFindCombiningSlot(consumable, out slot);
        }


        private static bool TryFindCombiningSlot(IConsumable consumable, out ISlot slot) =>
    (slot = _slots.FirstOrDefault(s => s.SlotType == SlotType.Combining && consumable.TryConsume(s.Item.Value))) != null;


    }
}
