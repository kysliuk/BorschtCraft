using System;
using System.Collections.Generic;
using System.Linq;

namespace BorschtCraft.Food
{
    public class CustomerOrderGenerator : ICustomerOrderGenerator
    {
        protected readonly ITableIngredientsList _tableIngredientsList;
        protected readonly Random _random = new Random();
        protected virtual bool GenerateNeedDrink => _random.NextDouble() > 0.5;

        protected IConsumed _dish;

        public CustomerOrder GenerateOrder()
        {
            var ingredients = GenerateIngredients();

            return new CustomerOrder(ingredients, GenerateNeedDrink);
        }

        protected IReadOnlyCollection<IConsumed> GenerateIngredients()
        {
            _dish = _tableIngredientsList.FirstLayer;
            var ingredientsAmount = _random.Next(1, _tableIngredientsList.IngredientsProviders.Count);

            for (int i = 0; i <= ingredientsAmount; i++)
            {
                var pickedIngredientProviderId = _random.Next(0, _tableIngredientsList.IngredientsProviders.Count);
                var pickedIngredientProvider = _tableIngredientsList.IngredientsProviders.Skip(pickedIngredientProviderId - 1).First();

                pickedIngredientProvider.TryConsume(_dish, out _dish);
            }

            List<IConsumed> ingredients = new List<IConsumed>
            {
                _dish
            };

            ingredients.AddRange(_dish.Ingredients);
            return ingredients;
        }

        public CustomerOrderGenerator(ITableIngredientsList tableIngredientList)
        {
            _tableIngredientsList = tableIngredientList;
        }
    }
}
