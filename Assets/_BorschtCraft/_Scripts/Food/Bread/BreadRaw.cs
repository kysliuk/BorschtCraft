namespace BorschtCraft.Food
{
    public class BreadRaw : Consumed, ICookable
    {
        public void Cook()
        {
            throw new System.NotImplementedException();
        }

        public BreadRaw(int price, Item item) : base(price, item)
        {
        }
    }
}
