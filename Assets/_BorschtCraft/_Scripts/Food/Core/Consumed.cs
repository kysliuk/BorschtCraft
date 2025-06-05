namespace BorschtCraft.Food
{
    public class Consumed : Item, IReleasable
    {
        protected Item _item;
        public virtual int Release()
        {
            return Price;
        }

        public Consumed(int price, Item item) : base(price)
        {
            _item = item;
        }
    }
}
