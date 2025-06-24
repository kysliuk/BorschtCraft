using System.Collections.Generic;

namespace BorschtCraft.Food
{
    public class CustomerOrder
    {
        public IReadOnlyCollection<IConsumed> Ingredients => _ingredients;
        public bool NeedDrink => _needDrink;

        private readonly bool _needDrink;
        private readonly IReadOnlyCollection<IConsumed> _ingredients;

        public CustomerOrder(IReadOnlyCollection<IConsumed> ingredients, bool needDrink)
        {
            _ingredients = ingredients;
            _needDrink = needDrink;
        }
    }
}
