using System.Collections.Generic;
using System.Linq;

namespace BorschtCraft.Food
{
    public class Consumed : Item, IConsumed
    {
        public IReadOnlyCollection<IConsumed> Ingredients => _ingredients;

        public IConsumed WrappedItem { get; private set; }
        protected HashSet<IConsumed> _ingredients;

        public bool HasIngredientOfType<T>() where T : Consumed
        {
            return _ingredients.OfType<T>().Any();
        }

        public Consumed(int price, IConsumed item) : base(item == null ? price : price + item.Price)
        {
            WrappedItem = item;
            if(_ingredients == null)
                _ingredients = new HashSet<IConsumed>();

            _ingredients.Add(item);
        }
    }
}
