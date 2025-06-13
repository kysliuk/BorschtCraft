namespace BorschtCraft.Food
{
    public class OnionStack : Consumable<Onion>
    {
        public override bool InnerCanDecorate(IConsumed item)
        {
            return item is ICooked;
        }

        public OnionStack(int price) : base(price)
        {
        }
    }
}
