namespace BorschtCraft.Food
{
    public class Consumable<T> : Item, IConsumable where T : IConsumed
    {
        public virtual IConsumed Consume(IConsumed item) => ConsumeAbstractFactory.CreateConsumed<T>(Price, item);

        public Consumable(int price) : base(price) { }
    }
}
