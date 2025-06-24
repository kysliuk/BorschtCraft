using System.Linq;
using Zenject;

namespace BorschtCraft.Food
{
    public class CombiningSlotReleasingHandler : SlotReleasingHandlerBase<ReleasingCombiningSlotStrategy>
    {
        protected override bool ProcessItemReleasing(ISlot slot)
        {
            var ingredients = slot.Item.Value.Ingredients.Select(i => i.GetType().Name);
            var result = slot.Item.Value.GetType().Name;

            foreach (var i in ingredients)
            {
                result += $" {i}";
            }
            Logger.LogWarning(this, $"Combining slot releasing handler is not implemented yet. Item of type {slot.GetType().Name} was released to customer from combining slot. Ingredients: {result}.");
            return true;
        }
    }
}
