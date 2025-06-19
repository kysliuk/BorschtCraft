namespace BorschtCraft.Food
{
    public class BreadCooked : Consumed, ICooked
    {
        public BreadCooked(int price, IConsumed item) : base(price, item)
        {
        }
    }
}
