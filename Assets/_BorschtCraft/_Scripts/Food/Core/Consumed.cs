namespace BorschtCraft.Food
{
    public class Consumed : Item, IReleasable
    {
        public virtual int Release()
        {
            return Price;
        }

        public Consumed(int price) : base(price)
        {
        }
    }
}
