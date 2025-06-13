namespace BorschtCraft.Food
{
    public class Garlic : Consumed, ISecondLayer
    {
        public Garlic(int price, IConsumed item) : base(price, item)
        {
        }
    }
}
