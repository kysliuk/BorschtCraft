using System;
using System.Collections.Generic;

namespace BorschtCraft.Food
{
    public abstract class TableIngredientListBase : ITableIngredientsList
    {
        public IReadOnlyCollection<IConsumable> IngredientsProviders => GetIngredientsProviders();
        public IDrink Drink => GetDrink();
        public IConsumed FirstLayer => GetFirstLayer();

        protected abstract IReadOnlyCollection<IConsumable> GetIngredientsProviders();

        protected abstract IDrink GetDrink();

        protected abstract IConsumed GetFirstLayer();
    }
}
