namespace BorschtCraft.Food
{
    public class BreadRaw : Consumed, ICookable<BreadCooked>
    {
        public BreadCooked Cook()
        {
            return BreadFactory.CreateConsumed<BreadCooked>(0, this);
        }

        public BreadRaw(int price, Item item) : base(price, item)
        {
        }
    }
}
