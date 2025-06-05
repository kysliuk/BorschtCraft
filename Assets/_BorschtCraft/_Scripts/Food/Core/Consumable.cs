namespace BorschtCraft.Food
{
    public class Consumable<T> : Item where T : Consumed
    {
        public virtual T Consume(Consumed item) => BreadFactory.CreateConsumed<T>(Price, item);

        public Consumable(int price) : base(price) { }
    }
}
