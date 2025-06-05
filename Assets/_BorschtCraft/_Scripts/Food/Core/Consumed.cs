using System.Collections.Generic;

namespace BorschtCraft.Food
{
    public class Consumed : Item, IReleasable
    {
        protected Item _item;
        protected HashSet<Item> _consumedItems;
        public virtual int Release()
        {
            return Price;
        }

        public Consumed(int price, Item item) : base(item == null ? price : price + item.Price)
        {
            _item = item;
            if(_consumedItems == null)
                _consumedItems = new HashSet<Item>();

            _consumedItems.Add(item);
        }
    }
}
