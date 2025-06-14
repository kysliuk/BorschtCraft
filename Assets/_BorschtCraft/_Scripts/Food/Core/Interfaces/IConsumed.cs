using System.Collections.Generic;

namespace BorschtCraft.Food
{
    public interface IConsumed
    {
        IConsumed WrappedItem { get; }

        public IReadOnlyCollection<IConsumed> Ingredients { get; }

        int Price { get; }
        bool HasIngredientOfType<T>() where T : IConsumed;
    }
}
