namespace BorschtCraft.Food
{
    public class Mustard : Consumed, IFourthLayer
    {
        public Mustard(int price, IConsumed item) : base(price, item)
        {
        }
    }
}
