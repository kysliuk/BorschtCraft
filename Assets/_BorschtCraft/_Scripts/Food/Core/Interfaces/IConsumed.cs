using System.Collections.Generic;

namespace BorschtCraft.Food
{
    public interface IConsumed
    {
        IConsumed WrappedItem { get; }
        int Price { get; }
        bool HasIngredientOfType<T>() where T : IConsumed;
    }
}
