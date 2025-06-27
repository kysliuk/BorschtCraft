using System;
using System.Linq;

namespace BorschtCraft.Food
{
    public class CustomerOrderGenerator : ICustomerOrderGenerator
    {
        private readonly ITableIngredientsList _tableIngredientsList;
        private readonly Random _random = new Random();

        protected virtual bool ShouldIncludeDrink => _random.NextDouble() > 0.5;

        public CustomerOrder[] GenerateOrders(int count)
        {
            var orders = new CustomerOrder[count];
            for (int i = 0; i < count; i++)
            {
                orders[i] = GenerateOrder();
            }

            return orders;
        }

        public CustomerOrder GenerateOrder()
        {
            var ingredients = GenerateDish();
            var drink = ShouldIncludeDrink ? _tableIngredientsList.Drink : null;
            return new CustomerOrder(ingredients, drink);
        }

        private IConsumed GenerateDish()
        {
            if (_tableIngredientsList.FirstLayer == null)
                throw new InvalidOperationException("First layer (base item) is not defined.");

            var ingredientProviders = _tableIngredientsList.IngredientsProviders?.ToList()
                ?? throw new InvalidOperationException("Ingredient providers list is null.");

            if (ingredientProviders.Count == 0)
                throw new InvalidOperationException("No ingredient providers available.");

            var dish = _tableIngredientsList.FirstLayer;
            int maxIngredients = ingredientProviders.Count;

            int ingredientCount = _random.Next(1, maxIngredients + 1);
            var selectedProviders = ingredientProviders
                .OrderBy(_ => _random.Next())
                .Take(ingredientCount);

            foreach (var provider in selectedProviders)
            {
                provider.TryConsume(dish, out dish);
            }

            return dish;
        }

        public CustomerOrderGenerator(ITableIngredientsList tableIngredientsList)
        {
            _tableIngredientsList = tableIngredientsList ?? throw new ArgumentNullException(nameof(tableIngredientsList));
        }

    }
}
