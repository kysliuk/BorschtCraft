namespace BorschtCraft.Food
{
    public class Burned : Consumed, IBurned
    {
        public Burned(int price, IConsumed wrappedItem) : base(0, wrappedItem)
        {

        }
    }
}
