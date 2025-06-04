namespace BorschtCraft.Food
{
    public class Consumable : Item
    {
        public virtual void Consume()
        {
        }

        public Consumable(int price) : base(price)
        {
        }
    }
}
