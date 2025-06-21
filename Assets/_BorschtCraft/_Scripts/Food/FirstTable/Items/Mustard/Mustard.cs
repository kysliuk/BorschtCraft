namespace BorschtCraft.Food
{
    public class Mustard : Consumed, IFifthLayer
    {
        public Mustard(int price, IConsumed item) : base(price, item)
        {
        }
    }
}
