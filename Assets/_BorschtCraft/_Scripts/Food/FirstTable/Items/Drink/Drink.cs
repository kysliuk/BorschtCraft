namespace BorschtCraft.Food
{
    public class Drink : Consumed
    {
        public Drink(int price, IConsumed wrappedItem) : base(price, wrappedItem)
        {
        }
    }
}