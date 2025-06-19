using System.Collections.Generic;

namespace BorschtCraft.Food
{
    public interface IConsumed : IItem
    {
        IConsumed WrappedItem { get; }
        public IReadOnlyCollection<IConsumed> Ingredients { get; }
        bool HasIngredientOfType<T>() where T : IConsumed;
    }
}
