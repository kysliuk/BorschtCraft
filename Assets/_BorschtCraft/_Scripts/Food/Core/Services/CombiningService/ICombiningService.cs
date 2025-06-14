using Zenject;

namespace BorschtCraft.Food
{
    public interface ICombiningService
    {
        /// <summary>
        /// Attempts to combine the given consumable with items in the releasing slots.
        /// </summary>
        /// <param name="decorator">The consumable item to use for decoration.</param>
        /// <param name="resultingItem">The item that results from the combination if successful, null otherwise.</param>
        /// <param name="decoratedSlot">The slot containing the item that was decorated if successful, null otherwise.</param>
        /// <returns>True if a combination was successful, false otherwise.</returns>
        bool AttemptCombination(IConsumable decorator, out IConsumed resultingItem, out IItemSlot decoratedSlot);
    }
}