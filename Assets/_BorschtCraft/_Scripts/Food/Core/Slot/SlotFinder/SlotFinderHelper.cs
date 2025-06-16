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

        public static ISlot FindCookingSlot()
        {
            return _slots.Where(s => s.SlotType == SlotType.Cooking && s.Item.Value == null).FirstOrDefault();
        }

        public static ISlot FindCombiningSlot(IConsumable consumable)
        {
            var item = consumable.TryConsume(null, out var succeed);
            return _slots.Where(s => s.SlotType == SlotType.Combining && consumable.TryConsume(s.Item.Value)).FirstOrDefault();
        }
    }
}
