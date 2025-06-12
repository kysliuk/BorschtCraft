namespace BorschtCraft.Food
{
    public class BreadRaw : Cookable<BreadCooked>
    {
        public BreadRaw(int price, Item item) : base(price, item)
        {
        }
    }
}
