using System.Linq;

namespace BorschtCraft.Food
{
    public class CookingSlotReleasingHandler : SlotReleasingHandlerBase<ReleasingCookingSlotStrategy>
    {
        protected override bool ProcessItemReleasing(ISlot slot)
        {
            Logger.LogInfo(this, $"Trying to place item of type {_consumed.GetType().Name} in combining slot.");

            var emptySlots = FindAnyEmptySlot();

            Logger.LogInfo(this, $"Found {emptySlots.Length} empty cooking slots.");

            foreach (var emptySlot in emptySlots)
            {
                var setted = emptySlot.TrySetItem(_consumed);

                if (setted)
                {
                    Logger.LogInfo(this, $"Item of type {_consumed.GetType().Name} was placed in combining slot.");
                    return setted;
                }
            }

            Logger.LogWarning(this, $"No empty combining slots available for item of type {_consumed.GetType().Name}.");
            return false;
        }

        private ISlot[] FindAnyEmptySlot()
        {
            return _slots.Where(slot => slot.SlotType == SlotType.Combining && !slot.Item.HasValue).ToArray();
        }
    }
}
