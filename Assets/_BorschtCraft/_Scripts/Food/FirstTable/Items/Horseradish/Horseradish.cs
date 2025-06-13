namespace BorschtCraft.Food
{
    public class Horseradish : Consumed, IFourthLayer
    {
        public Horseradish(int price, IConsumed item) : base(price, item)
        {
        }
    }
}
