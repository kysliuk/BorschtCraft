using System.Collections.Generic;

namespace BorschtCraft.Food
{
    public class CustomerOrder
    {
        public IReadOnlyCollection<IConsumed> Ingredients => _ingredients;
        public IDrink Drink => _drink;

        private readonly IDrink _drink;

        private readonly IReadOnlyCollection<IConsumed> _ingredients;

        public bool MathesIngredients(IConsumed item)
        {
            if (item == null)
                return false;

            return IngredientUtils.MatchItemToIngredients(item, _ingredients);
        }

        public CustomerOrder(IReadOnlyCollection<IConsumed> ingredients, IDrink drink)
        {
            _ingredients = ingredients;
            _drink = drink;
        }
    }
}
