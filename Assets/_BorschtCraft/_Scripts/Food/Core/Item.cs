namespace BorschtCraft.Food
{
    public abstract class Item
    {
        public int Price { get; private set; }

        public Item(int price)
        {
            Price = price;
        }
    }
}
