namespace BorschtCraft.Food
{
    public abstract class Cookable<T> : Consumed, ICookable where T : Consumed, ICooked
    {
        public float CookingTime { get; set; } = 3f;

        public virtual IConsumed Cook()
        {
            return ConsumeAbstractFactory.CreateConsumed<T>(0, this);
        }

        public Cookable(int price, IConsumed item) : base(price, item)
        {
        }
    }
}
