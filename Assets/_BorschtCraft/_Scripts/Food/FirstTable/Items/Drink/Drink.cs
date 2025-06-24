namespace BorschtCraft.Food
{
    public class Drink : Consumed, IDrink
    {
        public Drink(int price, IConsumed wrappedItem) : base(price, wrappedItem)
        {

        }
    }
}