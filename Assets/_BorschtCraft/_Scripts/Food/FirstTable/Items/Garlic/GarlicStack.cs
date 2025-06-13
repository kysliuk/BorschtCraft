namespace BorschtCraft.Food
{
    public class GarlicStack : Consumable<Garlic>
    {
        public override bool InnerCanDecorate(IConsumed item)
        {
            return item is ICooked;
        }

        public GarlicStack(int price) : base(price)
        {
        }
    }
}
