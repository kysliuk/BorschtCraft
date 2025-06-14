using System.Collections.Generic;
using System.Linq;

namespace BorschtCraft.Food
{
    public class Consumed : Item, IConsumed
    {
        private readonly HashSet<IConsumed> _ingredients = new HashSet<IConsumed>();

        public IReadOnlyCollection<IConsumed> Ingredients => _ingredients;
        public IConsumed WrappedItem { get; }


        public bool HasIngredientOfType<T>() where T : IConsumed
        {
            return _ingredients.OfType<T>().Any();
        }

        private void AddIngredientsRecursively(IConsumed item)
        {
            if (_ingredients.Add(item))
            {
                foreach (var ingredient in item.Ingredients)
                {
                    if (ingredient != null)
                        AddIngredientsRecursively(ingredient);
                }
            }
        }

        public Consumed(int price, IConsumed wrappedItem) : base(wrappedItem == null ? price : price + wrappedItem.Price)
        {
            WrappedItem = wrappedItem;

            if (wrappedItem != null)
                AddIngredientsRecursively(wrappedItem);
        }
    }
}
