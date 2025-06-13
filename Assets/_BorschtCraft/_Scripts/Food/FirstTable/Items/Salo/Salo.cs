namespace BorschtCraft.Food
{
    public class Salo : Consumed, IThirdLayer
    {
        public Salo(int price, IConsumed item) : base(price, item)
        {
        }
    }
}
