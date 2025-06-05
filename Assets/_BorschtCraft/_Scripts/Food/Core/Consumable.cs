namespace BorschtCraft.Food
{
    public class Consumable : Item
    {
        public T Consume<T>(int price) where T : Consumed => BreadFactory.CreateItem<T>(price, this);

        public Consumable(int price) : base(price) { }
    }
}
