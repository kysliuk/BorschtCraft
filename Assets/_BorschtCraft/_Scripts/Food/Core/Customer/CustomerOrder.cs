namespace BorschtCraft.Food
{
    public class CustomerOrder
    {
        public IConsumed Dish => _dish;
        public IDrink Drink => _drink;

        private readonly IDrink _drink;

        private readonly IConsumed _dish;

        public bool MathesIngredients(IConsumed item)
        {
            if (item == null)
                return false;

            return IngredientUtils.MatchItemsIngredients(item, _dish);
        }

        public CustomerOrder(IConsumed dish, IDrink drink)
        {
            _dish = dish;
            _drink = drink;
        }
    }
}
