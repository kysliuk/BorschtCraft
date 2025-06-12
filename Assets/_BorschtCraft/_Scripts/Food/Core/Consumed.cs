using System.Collections.Generic;
using System.Linq;

namespace BorschtCraft.Food
{
    public class Consumed : Item, IConsumed
    {
        public IReadOnlyCollection<Item> Ingredients => _ingredients;

        public Item WrappedItem { get; private set; }
        protected HashSet<Item> _ingredients;

        public bool HasIngredientOfType<T>() where T : Consumed
        {
            return _ingredients.OfType<T>().Any();
        }

        public Consumed(int price, Item item) : base(item == null ? price : price + item.Price)
        {
            WrappedItem = item;
            if(_ingredients == null)
                _ingredients = new HashSet<Item>();

            _ingredients.Add(item);
        }
    }
}
