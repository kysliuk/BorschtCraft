namespace BorschtCraft.Food
{
    public class Onion : Consumed, ISecondLayer
    {
        public Onion(int price, IConsumed item) : base(price, item)
        {
        }
    }
}
