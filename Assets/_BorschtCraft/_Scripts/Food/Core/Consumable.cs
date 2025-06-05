namespace BorschtCraft.Food
{
    public class Consumable : Item
    {
        public T Consume<T>(Consumed item) where T : Consumed => BreadFactory.CreateConsumed<T>(Price, item);

        public Consumable(int price) : base(price) { }
    }
}
