using System.Threading.Tasks;

namespace BorschtCraft.Food
{
    public class AnimatedSlotReleasingHandlerBase<T> : SlotReleasingHandlerBase<T> where T : ISlotMatchingStrategy
    {
        protected override async Task<bool> ProcessItemReleasing(ISlot slot)
        {
            var released = await base.ProcessItemReleasing(slot);

            if (released)
            {
                var fromSlotView = _slotViewRegistry.GetSlotView(slot);
                var toSlotView = _slotViewRegistry.GetSlotView(_itemDeliveredSignal.Slot);

                if (fromSlotView != null && toSlotView != null)
                {
                    await fromSlotView.MoveSlot(toSlotView);
                    Logger.LogInfo(this, $"Moved slot from {fromSlotView.name} to {toSlotView.name}");
                }
                else
                {
                    Logger.LogWarning(this, "One of the slot views is null. Cannot move slot.");
                }
            }

            return released;
        }
    }
}
