using Zenject;

namespace BorschtCraft.Food
{
    public class CombiningSlotReleasingHandler : SlotReleasingHandlerBase<CombiningSlotStrategy>
    {

        protected override bool ProcessItemReleasing(ISlot item)
        {
            //Logger.LogInfo(this, $"Item of type {item.GetType().Name} was released to customer from combining slot.");
            Logger.LogWarning(this, $"Combining slot releasing handler is not implemented yet. Item of type {item.GetType().Name} was released to customer from combining slot.");
            return true;
        }
    }
}
