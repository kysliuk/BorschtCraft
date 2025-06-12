namespace BorschtCraft.Food
{
    public abstract class Cookable<T> : Consumed, ICookable where T : Consumed, ICooked
    {
        public virtual IConsumed Cook()
        {
            return ConsumeAbstractFactory.CreateConsumed<T>(0, this);
        }

        public Cookable(int price, Item item) : base(price, item)
        {
        }
    }
}
