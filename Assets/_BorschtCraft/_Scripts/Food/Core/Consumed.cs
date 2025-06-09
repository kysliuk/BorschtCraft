using System.Collections.Generic;
using System.Linq;

namespace BorschtCraft.Food
{
    public class Consumed : Item
    {
        public IReadOnlyCollection<Item> Ingredients => _ingredients;

        protected Item _item;
        protected HashSet<Item> _ingredients;

        public bool HasIngredientOfType<T>() where T : Consumed
        {
            return _ingredients.OfType<T>().Any();
        }


        public Consumed(int price, Item item) : base(item == null ? price : price + item.Price)
        {
            _item = item;
            if(_ingredients == null)
                _ingredients = new HashSet<Item>();

            _ingredients.Add(item);
        }
    }
}
