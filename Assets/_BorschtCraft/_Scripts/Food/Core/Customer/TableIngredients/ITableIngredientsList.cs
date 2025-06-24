using System;
using System.Collections.Generic;

namespace BorschtCraft.Food
{
    public interface ITableIngredientsList
    {
        IReadOnlyCollection<IConsumable> IngredientsProviders { get; }
        IDrink Drink { get; }
        IConsumed FirstLayer {  get; }
    }
}
