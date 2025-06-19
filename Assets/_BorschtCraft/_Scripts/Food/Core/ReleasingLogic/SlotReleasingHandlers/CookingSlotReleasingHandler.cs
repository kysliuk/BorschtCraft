using System.Linq;

namespace BorschtCraft.Food
{
    public class CookingSlotReleasingHandler : SlotReleasingHandlerBase<ReleasingCookingSlotStrategy>
    {
        protected override bool CanHandle(IItem item)
        {
            if (_strategy.SlotType != SlotType.Cooking)
                return false;

            return base.CanHandle(item);
        }

        protected override bool ProcessItemReleasing(ISlot slot)
        {
            Logger.LogInfo(this, $"Trying to place item of type {_consumed.GetType().Name} in combining slot: {_slots.Length}.");

            var emptySlots = FindAnyEmptySlot();

            Logger.LogInfo(this, $"Found {emptySlots.Length} empty combining slots.");

            foreach (var emptySlot in emptySlots)
            {
                var setted = emptySlot.TrySetItem(_consumed);
                Logger.LogInfo(this, $"Trying to set item of type {_consumed.GetType().Name} in empty combining slot: {setted}.");

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
            return _slots.Where(slot => slot.SlotType == SlotType.Combining && slot.Item.Value == null).ToArray();
        }
    }
}
